using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Android.Content;
using Newtonsoft.Json;

namespace SQLite
{
    public class ApiResult
    {
        public string status;
        public string[] message;
    }

    [Activity(Label = "Test App", MainLauncher = true, Icon = "@drawable/RescYou")]
    public class MainActivity : Activity
    {
        EditText txtusername;
        EditText txtPassword;
        Button btncreate;
        Button btnsign;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.Main);
            // Get our button from the layout resource,  
            // and attach an event to it  
            btnsign = FindViewById<Button>(Resource.Id.btnlogin);
            btncreate = FindViewById<Button>(Resource.Id.btnregister);
            txtusername = FindViewById<EditText>(Resource.Id.txtusername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtpwd);
            btncreate.Click += Btncreate_Click;
            btnsign.Click += async delegate
            {
                using (var client = new HttpClient())
                {
                    // send a GET request  
                    var uri = "https://dog.ceo/api/breeds/list";
                    var result = await client.GetStringAsync(uri);

                    //handling the answer
                    var resultObject = JsonConvert.DeserializeObject<ApiResult>(result);

                    HomeSetup(resultObject);
                }

            };
            CreateDB();
        }
        private void Btncreate_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }
        private void HomeSetup(ApiResult item)
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<LoginTable>(); //Call Table  
                var usernameRow = data.Where(x => x.username == txtusername.Text && x.password == txtPassword.Text).FirstOrDefault(); //Linq Query  
                if (usernameRow != null)
                {
                    Toast.MakeText(this, "Login Success", ToastLength.Short).Show();
                    var serializedItem = JsonConvert.SerializeObject(item);
                    var homeIntent = new Intent(this, typeof(HomeActivity));
                    homeIntent.PutExtra("welcomeMessage", "Welcome, " + usernameRow.username);
                    homeIntent.PutExtra("intentData", serializedItem);
                    StartActivity(homeIntent);
                }
                else
                {
                    Toast.MakeText(this, "Username or Password invalid", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }
        public string CreateDB()
        {
            var output = "";
            output += "Creating Databse if it doesnt exists";
            string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Create New Database  
            var db = new SQLiteConnection(dpPath);
            output += "\n Database Created....";
            return output;
        }
    }
}

