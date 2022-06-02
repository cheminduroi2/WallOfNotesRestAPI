# WallOfNotesRestAPI

Simple ASP.NET Core/C# API for the Wall Of Notes web app.

Some example cURL calls that can be used to test the API:

***GET***

    curl https://localhost:5001/api/Notes

***POST***

    curl --header "Content-Type: application/json" \
    --request POST \
    --data '{"Name”:”newtester”,”Message":"this is a test message"}' \
    https://localhost:5001/api/Notes