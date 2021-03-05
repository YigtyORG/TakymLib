#!/bin/bash
echo begin: restore.sh
dotnet restore --packages=./packages
echo end: restore.sh
