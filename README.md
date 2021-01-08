# PassSaver
## How Use
### 1. dotnet PassSaver.dll <encrypt/decrypt> <file_name> password1 password2

### before encrypt, file pwords.txt contains not encrypted passwords

    yandex.ru login password
    mail.ru login password
    google.com login password

### after use this command
### dotnet bin/Debug/netcoreapp3.1/PassSaver.dll encrypt pwords.txt password password
### file pwords will contain this line
    LI70iAfTMxzevA8Woe1MrSoTeNYaR8X9FK3kH/kdEOfEBhhkqzDttJ7cmfsUCZ0acSTnzoTfbQ+pRZtP1MlZ75AuxgyR+dO96WPl1a6d8nE=
