#
# GAP - Generative Art Producer
#   by ZlomenyMesic & KryKom
#
#      founded 11.9.2024
#

# THIS SCRIPT IS USED TO RENAME VARIOUSY NAMED FILES USING A UNIFORM NAMING SYSTEM:
# the new names are formatted as: class.x.extension,
# where x represents the files order

import os

# the data is renamed, not copied!
DATA_DIR = r"C:\Users\michn\Desktop\convolutions\mixed_subsets\train\notladybug"

CLASS = "ladybug"
EXTENSION = "jpg"

i = 1
for name in os.listdir(DATA_DIR):
	src = DATA_DIR + "\\" + name
	dest = DATA_DIR + fr"\{CLASS}.{i}.{EXTENSION}"
	os.rename(src, dest)
	i += 1