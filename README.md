`Disclaimer:` I try to be discreet as much as possible about company but I might fail it at some point. ==Repository shall remain!==.

`SECOND IMPORTANT NOTE:` I use [Typora](https://typora.io/) for md file editing and I have a very customized setup for various MD format properties, like highlight, bold, italic text etc. if the text looks weird it is because of my custom CSS setup.

# CodeTest #7832

## Requirements

Write a C# application which shall make an application to a web server using Rest APIs

- special web server ready
- documentation and interfaces are provided
- register the application first and then create a CV like application and then apply to job.
- add information like contact info, attach documents, put some values and so on
- show operations in console

# Environment & Compilation

Application shall be in C#, NetCore3.1 (work environment). 

- open in VS2019 (my work environment) or another tool
- restore nuget packages
- compile

executing

- run app
- enter web API base URI, and then follow options available
- application does not store anything so everything has to be entered
- complete the job application or delete it

# Design

Application shall be C# NetCore Console application. it shall give freedom to user and user can do various operations at various times

Application shall not use parallelism, not necessary or required

Data validation is skipped, it is input related, and I did not do any validation about entries. only followed what APIs expected

# Known Issues & Design Decisions

API has some weird functions, maybe I misunderstood

- email/pwd is asked at first but then asked again. Not sure if I am supposed to send them again and again, maybe not. I can update password but still login with original password. I think it does not require password again but API asks it and I am not sure if it a new input or not. 
- JobApplication class is almost data agnostic. it does not care about the data, state of application, and does not store it. it is almost a pass-through from console to API. **I implemented it that way**
- because of above *stateless* design, attachments cannot be tracked. I did not store them. I did not track them
- I thought the JobApplication could make multiple applications for different people, I was wrong. it does not allow making another application once the application is done per generated user key. I updated the application to handle the situaiton but maybe it was not wanted or expected.
- once I could not make a new application because there was no attachment, but later it always accepted application without attachment. I do not know the actual rules of this
- the phone number has internal validation obviously, I do not know what is the expected value format there and it was not mentioned so I skipped data validation. same for email
- there was an API called *Find*, I did not need it or in the flow it was not necessary. I did not implement any use for it
- there is a file upload API. APIs doesnot provide information if data upload was successful, cannot test if it is working or not

