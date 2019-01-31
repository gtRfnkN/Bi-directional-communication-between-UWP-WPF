# Bi-directional communication between UWP & WPF

This project is a software architecture study and boilerplate used to combine a UWP & WPF app as a package with internal communication. The UWP app contains the UI of the project and is the entry point. It then starts the WPF app which can talk to external hardware and do elevated tasks. The communication between both parts is done with a bi-directional AppService.

## The Motivation for this project
Initially, I struggled to find a comprehensive guide which featured all points needed for us. Some guides also put the WPF app as an entry point and after stitching together multiple sources and approaches, the whole app just didn't run smoothly and the connection was lost randomly. This is why I wanted to build a minimal setup and here it is.

## Resources for this project
Most of the knowledge and input in this project is taken from [Stefan Wick's website](https://stefanwick.com/), especially his four-part guide on [UWP with Desktop Extension](https://stefanwick.com/2018/04/06/uwp-with-desktop-extension-part-1/). It's a great read and he does a fantastic job of explaining the setup. But I found some parts to be missing or not viable for us, so I wanted to create our own project.

One part, that I was searching for a bit, were the references used in the WPF app. Microsoft has two different lists of references needed, but I found that [this one from the Windows Blog](https://blogs.windows.com/buildingapps/2017/01/25/calling-windows-10-apis-desktop-application/#pZxlv72iZB3mKj2V.97) is the one that works.

## Contributing
If you find any bugs, problems or improvements, please feel free to submit an issue or create a pull request.

## License
[MIT](https://choosealicense.com/licenses/mit/)