#!/usr/bin/env bash

# Questo script fonde due texture di metallic e roughness in una sola texture di metallic + smoothness.
# E' il formato con cui vengono scaricati i materiali da polyhaven.com.
# Le texture originali devono avere le informazioni solo sul canale rosso.

# Su unity le texture per la proprietà metallic hanno sul canale Rosso le informazioi di metallic e sul 
# canale alfa le informazioni sulla smoothness. La smoothness è 1-roughness da qui il -negate nel comando.

metallic=$1
roughness=$2
output=$3
[[ -z $output ]] && output="result.exr"

# Determino l'estensione finale, in caso di EXR applico la compressione. 
# ${extension,,} serve a mettere in lowercase l'estensione
filename=$(basename -- "$output")
extension="${filename##*.}"
[[ ${extension,,} == "exr" ]] && compression="-compress PIZ"         

# DESCRIZIONE COMANDO:
#  - Salva la roughness in memoria e cancellala dalla lista,
#  - Prendi la mettallic e estrai il Rosso come nuovo rosso.
#  - Il nuovo G e B sono messi a zero.
#  - Il nuovo alfa è preso negando il rosso della roughness, ottenendo la smoothness.
#  - Con l'operatore "-combine" costruiamo una nuova immagine usando ciascuna immagine della lista precedente per il rispettivo canale.


echo "Merging Textures.."
echo "Metallic: $metallic"
echo "Roughness: $roughness"
echo "Output: $output"

magick.exe $roughness -write MPR:rough +delete $metallic -channel R -separate \
  \( MPR:rough -threshold 100% -alpha off \) \
  \( MPR:rough -threshold 100% -alpha off \) \
  \( MPR:rough -channel R -separate -negate \) \
  $compression -combine $output
  
# -define exr:color-type=RGBA       # Specify the color type for the EXR format: RGB, RGBA, YC, YCA, Y, YA, R, G, B, A).