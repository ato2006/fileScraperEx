# fileScraperEx
fileScraperEx is an automated tool to automatically scrape links and files from exported Discord conversations. It'll automatically download any files found in private chats and save them in a directory.

# Installation
.NET 5 or newer is required to run fileScraperEx. You can either compile from source or use the provided binaries in the Releases section which includes default file extensions and URL blacklists.

# FAQ
## How do I use this?
You can specify all file extensions that will be searched for in a file called `Extensions.txt` that resides in the same directory as the binary. They have to be in the format `.extension`. For actual usage, please use the `-h` flag when starting the program.

## How can I provide custom blacklists?
If you want to specify custom blacklisted urls, edit the `Blacklist.txt` file. Make sure to enable blacklisted terms with the `-b` flag.

## What other options/flags are there?
Use `-l` to include links in the output. Use `-v` to enable verbose output. Use `-e` to provide a custom file extension file (for whatever reason).  
