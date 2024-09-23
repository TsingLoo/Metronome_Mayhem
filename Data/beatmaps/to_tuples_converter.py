import json
import os
from pathlib import Path


def convert_json_to_tuples(json_file):
    with open(json_file, 'r') as f:
        beatmap_data = json.load(f)

    tuple_format_data = []

    for entry in beatmap_data:
        hold = entry['hold']
        beat = entry['beat']
        key = entry['key']
        tuple_format_data.append((hold, beat, key))

    return tuple_format_data


def main():
    # Song select for conversion
    songs_path = Path(f"./Data/beatmaps").resolve()
    mapped_songs = list(song for song in os.listdir(songs_path) if '.' not in song)
    mapped_songs_str = '\n'.join(mapped_songs)
    song_name = ' '
    while song_name not in mapped_songs:
        song_name = input(f"Select song name from the following:\n{mapped_songs_str}\n")

    # Set path
    json_path = Path(f"./Data/beatmaps/{song_name}/{song_name}.json").resolve()
    tuple_path = Path(f"./Data/beatmaps/{song_name}/{song_name}.txt").resolve()

    # Run conversion
    tuple_beatmap = convert_json_to_tuples(json_path)

    # Save result to a new text file
    with open(tuple_path, mode='w') as tuple_file:
        for note in tuple_beatmap:
            tuple_file.write(f"{note} ")

    print("Conversion complete!")


if __name__ == "__main__":
    main()
