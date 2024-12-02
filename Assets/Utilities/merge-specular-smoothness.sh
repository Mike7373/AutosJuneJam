#!/usr/bin/env bash

# Questo script fonde una texture specular\glossiness e una di smoothness in una sola texture di specular + smoothness.
# In sostanza mette la smoothness nel canale alfa della nuova texture e per l'RGB usa la specular
# Se stiamo usando dei png, mantiene l'uso dei png, altrimenti utilizza exr compresso


specular=$1
smoothness=$2
output=$3
[[ -z $output ]] && output="result.exr"

# Determino l'estensione finale, in caso di EXR applico la compressione. 
# ${extension,,} serve a mettere in lowercase l'estensione
filename=$(basename -- "$output")
extension="${filename##*.}"
[[ ${extension,,} == "exr" ]] && compression="-compress PIZ"           

echo "Merging Textures.."
echo "Specular: $specular"
echo "Smoothness\Glossiness: $smoothness"
echo "Output: $output"
echo "Output Format: $extension"

magick.exe  $specular $smoothness -compose copy-opacity -composite $compression $output

  