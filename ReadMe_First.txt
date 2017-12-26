++++++++++++++++++++++++++++++++++ Database Part  ++++++++++++++++++++++++++++++++++

||||||||||||||||||||||||| == Author :- Saineshwar Bageri == |||||||||||||||||||||||||

====================================================================================

1) First thing to do is Create Database with Name :- EventDB

====================================================================================

2) After Creating Database now make changes ConnectionStrings in appsettings.json file

  Change this connectionStrings your Own Data Source and Sql UserName and Password.

 ## Sql Server Authentication ##

  "ConnectionStrings":
  {
    "DatabaseConnection": "Data Source=SAI-PC; UID=sa; Password=Pass@123;Database=EventDB;"
  }

 ## Windows Authentication ##
  
  "ConnectionStrings": 
   {       
      "DatabaseConnection": "Server=YOURSERVERNAME; 
      Database=YOURDATABASENAME;
      Trusted_Connection=True; 
      MultipleActiveResultSets=true"        
   } 


====================================================================================
  
3) Login Details
 
   1) Customer
       UserID : Customer
       Password : 123456

  2) Admin
      UserID : Admin
       Password : 123456

  3) SuperAdmin
      UserID : SuperAdmin
       Password : 123456

====================================================================================