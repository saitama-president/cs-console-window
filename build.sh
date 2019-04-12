#! /bin/bash

mcs \
./main.cs \
./EscapeSequence.cs \
./Window.cs ;


if [ $? -lt 0 ]
then
  echo "ERROR";
else
  echo "OK";
  mono main.exe;
fi
