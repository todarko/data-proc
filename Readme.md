# data-proc

## This project is the processing of the same data files in different languages

Currently used languages include C#, python, and rust

Each project I've tried to roughly use the same methods. Python was around 2.8s, C# ~.4s, and rust was ~.1s, on average

## Files used

DevicesWithInventoryUsernameOnly.csv is a file that contains dummy emails that if matched to the other file would be excluded from the output

InteractiveSignIns.csv is a modified output of Windows AD logins, if the emails are not matched to the other file, then it will create a unique list of all the results and write it to a new csv in the output folder.