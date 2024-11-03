#
# GAP - Generative Art Producer
#   by ZlomenyMesic & KryKom
#
#      founded 11.9.2024
#

import os
import shutil
import pathlib

ORIGINAL_DIR = pathlib.Path(r"C:\Users\michn\Desktop\convolutions\mixed")
DATASET_DIR = pathlib.Path(r"C:\Users\michn\Desktop\convolutions\mixed_subsets")

# used to split the whole dataset into three batches - training, validation and testing
# this might not be needed later, however it's important at the moment
def create_subset(subset_name, index_start, index_end):
    for catg in ("ladybug", "notladybug"):
        dir = DATASET_DIR / subset_name / catg
        os.makedirs(dir)

        f_names = [f"{catg}.{i}.jpeg" for i in range(index_start, index_end)]
        for f_name in f_names:
            shutil.copyfile(src = ORIGINAL_DIR / f_name, dst = dir / f_name)

create_subset("train", 1, 1350)
print("subset \"train\" created")

create_subset("validation", 1350, 1395)
print("subset \"validation\" created")

create_subset("test", 1395, 1397)
print("subset \"test\" created")