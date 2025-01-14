#!/usr/bin/env python3
import os
import subprocess
import sys
from pathlib import Path
from dotenv import load_dotenv

def run_command(command):
    """Run a shell command and exit if it fails."""
    result = subprocess.run(command, shell=True)
    if result.returncode != 0:
        sys.exit(result.returncode)

def get_latest_package(directory):
    """Get the latest .nupkg file from the specified directory."""
    packages = list(Path(directory).glob('*.nupkg'))
    if not packages:
        print("Error: No .nupkg files found.")
        sys.exit(1)
    return max(packages, key=os.path.getctime)

def main():
    load_dotenv()

    nuget_api_key = os.getenv('NUGET_API_KEY')
    if not nuget_api_key:
        print("Error: NUGET_API_KEY environment variable is not set.")
        sys.exit(1)

    run_command('dotnet build -c Release src/')

    run_command('dotnet pack -c Release src/')

    package_path = get_latest_package('src/Monads/bin/Release')

    run_command(f'dotnet nuget push {package_path} -k {nuget_api_key} -s https://api.nuget.org/v3/index.json')

if __name__ == '__main__':
    main()