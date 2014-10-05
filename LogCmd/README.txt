This program will look at the %ERRORLEVEL% after a batch file has completed.

This %ERRORLEVEL% variable can only be returned back to this program if the last line is equal to 'EXIT /B %ERRORLEVEL%'

If %ERRORLEVEL% is set to anything other than a successful value, the program will attempt to send an email containing any error information.
	-	This relies on an email server being specified inside this programs App.config file.