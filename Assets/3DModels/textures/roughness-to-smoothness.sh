#!/usr/bin/env bash

# Prende una singola texture di roughness e la trasforma in una metallic+smoothness con la metallic
# vuota e la smoothness sull'alfa.

roughness=$1
output=$2
[[ -z $output ]] && output="result.exr"

# DESCRIZIONE COMANDO:
# Prende la texture di roughness e costruisce le 4 immagini per i 4 canali della nuova immagine:
# Il rosso, verde e blu vengono presi dal verde della roughness, che è tutto a zero.
# L'alfa diventa la smoothness prendendo il canale rosso della roughness e negandolo

filename=$(basename -- "$output")
extension="${filename##*.}"
[[ ${extension,,} == "exr" ]] && compression="-compress PIZ"     


echo "Building Texture.."
echo "Roughness: $roughness"
echo "Output: $output"

magick.exe $roughness -write MPR:rough -threshold 100% -alpha off \
  \( MPR:rough -threshold 100% -alpha off \) \
  \( MPR:rough -threshold 100% -alpha off \) \
  \( MPR:rough -channel R -separate -negate \) \
  $compression -combine $output
 
# -define exr:color-type=RGBA       # Specify the color type for the EXR format: RGB, RGBA, YC, YCA, Y, YA, R, G, B, A).