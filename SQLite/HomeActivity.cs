using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Views.ViewGroup;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SQLite
{
    [Activity(Label = "Home")]
    public class HomeActivity : Activity
    {
       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string text = Intent.GetStringExtra("intentData") ?? "User";
            SetContentView(Resource.Layout.Home);
            TextView currentCharacterName = FindViewById<TextView>(Resource.Id.welcomeMesssage);
            currentCharacterName.Text = text;
            int id = 1;
            RelativeLayout relLayout = new RelativeLayout(this);
            var textView1 = new TextView(this) { Id = id, TextSize = (float)50.0, Text = Intent.GetStringExtra("welcomeMessage") ?? "User" };
            var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent);
            param.AddRule(LayoutRules.AlignParentTop);
            relLayout.AddView(textView1, param);

            ApiResult deserializedItem =
                JsonConvert.DeserializeObject<ApiResult>(Intent.GetStringExtra("intentData"));

            foreach (var item in deserializedItem.message)
            {
                var textView2 = new TextView(this) { Id = ++id, Text = item };
                param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent);
                param.AddRule(LayoutRules.Below, id-1);
                relLayout.AddView(textView2, param);
            }
            SetContentView(relLayout);
        }
    }
}