#
# GAP - Generative Art Producer
#   by ZlomenyMesic & KryKom
#
#      founded 11.9.2024
#

# THIS SCRIPT IS USED TO SPLIT THE AVAILABLE TRAINING DATA INTO THREE SUBSETS:
#   1. TRAINING   - directly modifies weights
#   2. VALIDATION - monitors generalization abilites and potential overfitting
#   3. TESTING    - real unknown data simulation

import os
import shutil
import pathlib

# source of the available dataset
ORIGINAL_DIR = pathlib.Path(r"C:\Users\michn\Desktop\convolutions\mixed")
# subsets output directory
SUBSET_DIR = pathlib.Path(r"C:\Users\michn\Desktop\convolutions\mixed_subsets")

# creates subsets for binary classification problems
BINARY_CLASS_1 = "ladybug"
BINARY_CLASS_2 = "notladybug"
def create_subset_binary_class(subset_name, index_start, index_end):
    for catg in (BINARY_CLASS_1, BINARY_CLASS_2):
        dir = SUBSET_DIR / subset_name / catg
        os.makedirs(dir)

        f_names = [f"{catg}.{i}.jpeg" for i in range(index_start, index_end)]
        for f_name in f_names:
            shutil.copyfile(src = ORIGINAL_DIR / f_name, dst = dir / f_name)


# GENERATE SUBSETS HERE:

create_subset("train", 1, 1350)
print("subset \"train\" created")

create_subset("validation", 1350, 1395)
print("subset \"validation\" created")

create_subset("test", 1395, 1397)
print("subset \"test\" created")