﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Common.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Common
{
	public class ShowdownRESTClient : IAnnoucementInteractor
    {
        HttpClient client;

        public String Token { get; set; }

		public ShowdownRESTClient()
		{
			client = new HttpClient();
		}

        public async Task<string> GetToken(string facebookAccessToken)
        {
			string jsonString = JsonConvert.SerializeObject(new { facebookAccessToken = facebookAccessToken }, Formatting.None);
            string response = await PostAsync("/accounts/login", jsonString);

            var data = JObject.Parse(response);
			return ((string)data["token"]);
        }

        public async Task<String> GetTwitterQueryAsync()
        {
            var isBackendQueryRequestImplemented = await Connectivity.IsBackendReachable("/twitter/query");
            //This didn't work.. sooo 
            isBackendQueryRequestImplemented = false;

            String query;
            if (isBackendQueryRequestImplemented)
            {
                query = await RequestAsync("/twitter/query");
            }
            else
            {
                String HASHTAG_PRIMARY = "#TxShowdown18";
				String HASHTAG_SECONDARY = "#RaceToShowdown2018";
                String BASE_SEARCH_URL = "https://twitter.com/search";

                var uribuilder = new UriBuilder(BASE_SEARCH_URL);
                    
                var queryBuilder = HttpUtility.ParseQueryString(uribuilder.Query);
                queryBuilder["f"] = "tweets";
                queryBuilder["vertical"] = "default";
                queryBuilder["q"] = HASHTAG_PRIMARY + ", OR " + HASHTAG_SECONDARY;
				queryBuilder["src"] = "typd";
                uribuilder.Query = queryBuilder.ToString();
                query = uribuilder.Uri.ToString();
            }
            return query;
        }

        public async Task<List<Event>> GetScheduleAsync()
		{
			var jsonString = await RequestAsync("/events/schedule");
			var eventList = Event.FromJSONArray(jsonString);
			return SetEventsToCst(eventList);
		}

		public async Task<Location> GetLocationInformation(string id)
		{
			var path = $"/events/locations/{id}";
			var jsonString = await RequestAsync(path);
			return Location.FromJSON(jsonString);
		}

        public async Task<List<Announcement>> GetAnnouncements()
        {
			var jsonString = await RequestAsync("/notifications/annoucements");
			return Announcement.FromJSONArray(jsonString);
        }

        public async Task<List<Game>> GetAllGames()
        {
            var jsonString = await RequestAsync("/scores/games");
            return Game.FromJSONArray(jsonString);
        }

        public async Task<List<Game>> GetEventGames(string eventId)
        {
            var path = $"/events/{eventId}/games";
            var jsonString = await RequestAsync(path);
            return Game.FromJSONArray(jsonString);
        }

		public async Task<List<Game>> GetSchoolGames(string schoolSlug)
		{
			var path = $"/core/schools/{schoolSlug}/games";
			var jsonString = await RequestAsync(path);
			return Game.FromJSONArray(jsonString);
		}

        public async Task<List<Score>> GetScoreHistory(String gameId)
        {
            var path = $"/scores/games/{gameId}/scores";
            var jsonString = await RequestAsync(path);
            return Score.FromJSONArray(jsonString);
        }

		private async Task<string> RequestAsync(string path)
		{
			var url = $"{Secrets.BACKEND_URL}{path}.json";
			var response = await client.GetAsync(url);
			return await response.Content.ReadAsStringAsync();
		}

        private async Task<string> PostAsync(string path, string jsonString)
        {
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var url = $"{Secrets.BACKEND_URL}{path}.json";
            var response = await client.PostAsync(url, content);
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception(response.StatusCode.ToString());
            return await response.Content.ReadAsStringAsync();
        }

		public Task<Announcement> CreateAnnouncement(Announcement announcement)
		{
			throw new NotImplementedException();
		}

		private List<Event> SetEventsToCst(List<Event> events)
		{
			List<Event> fixedEvent = new List<Event>();
			TimeZoneInfo cstInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");
			for(int i = 0; i < events.Count; i++)
			{
				Event e = events[i];
				e.StartTime = TimeZoneInfo.ConvertTimeFromUtc(e.StartTime.DateTime, cstInfo);
				e.EndTime = TimeZoneInfo.ConvertTimeFromUtc(e.EndTime.DateTime, cstInfo);
				fixedEvent.Add(e);
			}
			return fixedEvent;
		}
	}
}
