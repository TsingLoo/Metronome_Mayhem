import json
import os
from pathlib import Path


def osu_to_json(osu_file, bpm: int):
    def time_to_beat(offset, bpm):
        return (offset / 1000) * (bpm / 60)

    x_to_key = {
        64: "D",   # Osu!mania x-position 64 → D key
        192: "F",  # Osu!mania x-position 192 → F key
        320: "J",  # Osu!mania x-position 320 → J key
        448: "K"   # Osu!mania x-position 448 → K key
    }
    beatmap_data = []

    with open(osu_file, 'r') as file:
        lines = file.readlines()

        # Find where hit objects start
        hit_objects_started = False
        for line in lines:
            line = line.strip()
            if line == "[HitObjects]":
                hit_objects_started = True
                continue

            if hit_objects_started and line:
                note_data = line.split(',')

                x_pos = int(note_data[0])
                timestamp = int(note_data[2])

                # Detecting holds (5th value if present is the end time for a hold)
                hold_end_time = int(note_data[5].split(":")[0]) if len(note_data) > 5 else None

                # Convert the data into beat and hold
                beat = time_to_beat(timestamp, bpm)
                hold = time_to_beat(hold_end_time, bpm) - beat if hold_end_time else 0

                # Map x_pos to keys
                key = x_to_key.get(x_pos)
                if key is None:
                    print(f"Warning: Invalid key with x-position {x_pos}\nSet to default: 'X'")
                    key = "X"

                beatmap_data.append({
                    "beat": round(beat, 2),
                    "hold": round(hold, 2),
                    "key": key
                })
    return beatmap_data


def main():
    # Song select for conversion
    songs_path = Path(f"./Data/beatmaps").resolve()
    mapped_songs = '\n'.join(os.listdir(songs_path))
    song_name = ' '
    while song_name not in mapped_songs:
        song_name = input(f"Select song name from the following:\n{mapped_songs}\n")

    # Run conversion
    osu_file_path = Path(f"./Data/beatmaps/{song_name}/{song_name}.osu").resolve()
    beatmap_json = osu_to_json(osu_file_path, 138)

    # Save to JSON file
    json_path = Path(f"./Data/beatmaps/{song_name}/{song_name}.json").resolve()
    with open(json_path, 'w') as json_file:
        json.dump(beatmap_json, json_file, indent=4)

    print("Conversion complete!")


if __name__ == "__main__":
    main()
