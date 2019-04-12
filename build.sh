#! /bin/bash

mcs \
./test.cs \
./Common.cs \
./Screen.cs \
./EscapeSequence.cs \
./Window.cs \
-out:test.exe;


if [ $? -lt 0 ]
then
  echo "ERROR";
else
  echo "OK";
  mono test.exe;
fi
