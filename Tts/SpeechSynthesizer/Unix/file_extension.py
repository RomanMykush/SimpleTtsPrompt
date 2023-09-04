import os
import platform

def path_seperator():
    if platform.system() == "Windows":
        return '\\'
    return '/'

def create_directory(path: str, doCheck = True):
    if os.path.isdir(path):
        return
    if not doCheck or platform.system() == "Windows":
        os.mkdir(path)
        return
    create_path(path, True)
    create_directory(path, False)

def create_file(path: str, doCheck = True):
    if os.path.isfile(path):
        return
    if not doCheck or platform.system() == "Windows":
        os.mknod(path)
        return
    create_path(path, True)
    create_file(path, False)

def create_path(path: str, ignoreLast: bool):
    # Getting path elements
    seperator = path_seperator()
    path_elements = path.split(seperator)
    # Check for drive/root
    if platform.system() == "Windows" and ':' in path_elements[0]:
        path_elements[1] = path_elements[0] + seperator + path_elements[1]
        path_elements = path_elements[1:]
    elif not path_elements[0]:
        path_elements = path_elements[1:]
        path_elements[0] = seperator + path_elements[0]
    # Ignore last element
    if ignoreLast:
        path_elements = path_elements[:-1]
    # Check lise is empty
    if not path_elements:
        return
    # Creation of path elements except of last one
    current = path_elements[0]
    create_directory(current, False)
    for dir in path_elements[1:]:
        current += seperator + dir
        create_directory(current, False)