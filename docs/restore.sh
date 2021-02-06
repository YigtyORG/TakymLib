#!/bin/bash
echo begin: restore.cmd
dotnet restore --packages=./packages
echo end: restore.cmd
