# Images extractor and top 10 mos used words counter

This is a very simple Rest API which has the purpose of extract all images and to 10 most used words from a given URL by using .NET core 3.1 and Selenium with chrome webdriver.


## Install
1 - Download and install the .Net Core 3.1 according to your operating system.
https://dotnet.microsoft.com/download/dotnet/3.1

2 - Clone this repository

    git clone https://github.com/apandrade/API.extractor.git

3 - Download chrome webdrive for selenium according to your current installed chrome version and extract the chromedriver.exe into a folder of your choice

https://sites.google.com/a/chromium.org/chromedriver/downloads

4 - Open the cloned project in visual studio and open the launchSettings.json file located inside Properties folder and change the value of CHROME_WEBDRIVER_PATH environment variable to the same path where you put the chromedrive.exe on the previous step.
    
    "CHROME_WEBDRIVER_PATH": "C:\\WebDriver\\bin"
## Run the app
### Command Prompt
Open a window command prompt, navigate to the project folder API.extractor\API.Extractor and run 
    
    dotnet run API.Extractor

Now open your web browser and navigate to https://localhost:5001/swagger/index.html to see the swagger API documentation


## Run the tests

Open a window command prompt, navigate to the project root folder API.extractor and run

    dotnet test UnitTests

## API Instructions
There is just one post method on route

    api/v1/extractor

The payload expected is the json below where url is the website you want scraping and download is the boolean that indicates if the api should or not download all images for server, if download is false the api will returns the originals urls of images

    {
        "Url": "https://giphy.com/gifs/brazil-PSKAppO2LH56w",
        "download": "true"
    }
