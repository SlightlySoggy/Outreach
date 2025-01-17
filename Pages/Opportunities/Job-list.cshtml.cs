﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Outreach.Pages.Utilities;

namespace Outreach.Pages.Opportunities
{
    public class JoblistModel : PageModel
    {
         
        public List<Opportunity> oppList { get; set; }
        public List<Tag> ListTag = new List<Tag>();
        public List<string> SelectedTagIds = new List<string>();
        public string defaultsearchtext = "";
        public void OnGet()
        {
            GeneralUtilities ut = new GeneralUtilities(); 

            if (Request.Query["Searchkeyword"].ToString() != "")
            {
                string txtSearch = Request.Query["Searchkeyword"];
                oppList = ut.getOpportunityListByCondition(txtSearch, "");

                defaultsearchtext = txtSearch;
            }
            else
            {
                 oppList = new List<Opportunity> { };
            }

            Tag tag = new Tag();
            ListTag = tag.GetReferencedTagsbyOpptunityId(""); // get all active tags
        }

        //public IActionResult OnPostButton1(IFormCollection data)
        //{
        //    //...
        //}


        //public IActionResult OnPostButton2(IFormCollection data)
        //{
        //    //...
        //}

        public void OnPost()
        {
            // https://www.learnrazorpages.com/razor-pages/handler-methods

            string txtSearch = "";
            if (Request.Form["TxtSearch"] != "")
            {
                defaultsearchtext = Request.Form["TxtSearch"]; 
            }   

            GeneralUtilities ut = new GeneralUtilities();

            if (txtSearch != null || Request.Form["chkTag"] != "")
            {
                SelectedTagIds = Request.Form["chkTag"].ToString().Split(',').ToList();
                oppList = ut.getOpportunityListByCondition(defaultsearchtext, Request.Form["chkTag"], Request.Form["chkorderby"]);
            }
             

            Tag tag = new Tag();
            ListTag = tag.GetReferencedTagsbyOpptunityId(""); // get all active tags
        }
        //Response.Redirect("Index");
    }
}
 