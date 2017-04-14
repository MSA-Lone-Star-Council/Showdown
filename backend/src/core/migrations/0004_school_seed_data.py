# -*- coding: utf-8 -*-
# Generated by Django 1.10.6 on 2017-04-07 02:12
from __future__ import unicode_literals

from django.db import migrations

def generate_school_seed_data(apps, schema_editor):
    School = apps.get_model("core", "School")
    db_alias = schema_editor.connection.alias

    School.objects.using(db_alias).bulk_create([
        School(
            name="The University of Texas at Dallas",
            short_name="UT Dallas",
            slug="ut-dallas",
            logo="http://example.com",
        ),
        School(
            name="The University of Texas at Austin",
            short_name="UT Austin",
            slug="ut-austin",
            logo="http://example.com",
        ),
        School(
            name="The University of Texas at Arlington",
            short_name="UT Arlington",
            slug="ut-arlington",
            logo="http://example.com",
        ),
        School(
            name="University of Houston",
            short_name="UH",
            slug="univ-houston",
            logo="http://example.com",
        ),
        School(
            name="Texas A&M University at College Station",
            short_name="A&M",
            slug="tamu-cstat",
            logo="http://example.com",
        ),
        School(
            name="The University of Texas at San Antonio",
            short_name="UT SA",
            slug="utsa",
            logo="http://example.com",
        ),
        School(
            name="University of North Texas",
            short_name="UNT",
            slug="unt",
            logo="http://example.com",
        ),
        School(
            name="Southern Methodist University",
            short_name="SMU",
            slug="smu",
            logo="http://example.com",
        ),
        School(
            name="The University of Texas at Tyler",
            short_name="UT Tyler",
            slug="ut-tyler",
            logo="http://example.com",
        ),
        School(
            name="Texas Tech",
            short_name="Tech",
            slug="texas-tech",
            logo="http://example.com",
        ),
        School(
            name="Wharton Junior Community College",
            short_name="Wharton",
            slug="wharton-junior-cc",
            logo="http://example.com",
        ),
        School(
            name="The University of Texas at Rio Grande",
            short_name="UT RG",
            slug="ut-rg",
            logo="http://example.com",
        ),
        School(
            name="Collins College",
            short_name="Collins",
            slug="collins-college",
            logo="http://example.com",
        ),
        School(
            name="Rice University",
            short_name="Rice",
            slug="rice",
            logo="http://example.com",
        ),
        School(
            name="Richland College",
            short_name="Richland",
            slug="richland-college",
            logo="http://example.com",
        ),
        School(
            name="Brookhaven Community College",
            short_name="Brookhaven",
            slug="brookhaven-cc",
            logo="http://example.com",
        ),
        School(
            name="Texas State University",
            short_name="Texas State",
            slug="texas-state",
            logo="http://example.com",
        ),
    ])


class Migration(migrations.Migration):

    dependencies = [
        ('core', '0003_school_short_name'),
    ]

    operations = [
        migrations.RunPython(generate_school_seed_data),
    ]
