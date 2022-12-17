using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Outreach.Data;
using Outreach.Pages.Opportunities;
using Outreach.Pages.Utilities;

namespace Outreach.Areas.Consoles.Pages.Content.Tools.Projects.ProjectAdd  
{
    [Authorize]//(Roles = "OrganizationContactor")]
    public class ProjectsAddLightModel : PageModel
    {
        public Organization orgInfo = new Organization();
        public Project projectInfo = new Project();
        //public List<PTStatus> ListTag = new List<PTStatus>(); //Project Task Status 

        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<JobdetailModel> _logger;


        public int org_id = 2;
        public int user_id = 0;
        public string errorMessage = "";
        public string successMessage = "";

        public ProjectsAddLightModel(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              ILogger<JobdetailModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        //public void OnGet()
        //{
        //}


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to find signin user info.");
                return Page();
            }
            GeneralUtilities generalUtilities = new GeneralUtilities();
            user_id = generalUtilities.GetLoginUserIntIDbyGUID(user.Id);

            //user_id = Convert.ToInt32(user.User_Id);
            projectInfo.CreatedOrgId = org_id.ToString();
            projectInfo.CreatedUserId = user_id.ToString();


            //PTStatus ptStatus = new PTStatus();
            //ListTag = PTStatus.GetReferencedTagsbyOpptunityId(""); // get all active Status

            //orgInfo = GetOrganizationInfoByOrg_id();
            if (orgInfo == null)
            {

            }

            if (string.IsNullOrWhiteSpace(Request.Query["ProjectId"]))
            { // create a brand new Project 
                return Page();
            }
            else
            { // load Project based on given id
                String ProjectId = Request.Query["ProjectId"];
                Project op = new Project(ProjectId);
                projectInfo = op;
            }

            return Page();

        }
        public void OnPost()
        {


            projectInfo.ProjectName = Request.Form["inputName"];
            projectInfo.Description = Request.Form["inputDescription"];
            projectInfo.EstimatedBudget = Request.Form["inputEstimatedBudget"];
            projectInfo.ActualSpent = Request.Form["inputSpentBudget"];
            projectInfo.CreatedOrgId = Request.Form["hid_orgid"];  
            projectInfo.CreatedDate = DateTime.Now.ToString();
            projectInfo.CreatedUserId = Request.Form["hid_userId"];
            projectInfo.ProjectManagerUserId = Request.Form["hid_userId"];
            projectInfo.StartDate = Request.Form["inputStartDate"];
            projectInfo.DueDate = Request.Form["inputDueDate"];
            projectInfo.CompletionDate = Request.Form["inputCompletionDate"];
            projectInfo.DurationByDay = Request.Form["inputEstimatedDuration"];
            projectInfo.ProjectTaskStatusId = "1";
             

            string result = "";

            //if (!string.IsNullOrWhiteSpace(Request.Form["chktag"]))
            //{ // multiple selction value can be [4,12,22]
            //    List<string> listTagid = Request.Form["chktag"].ToString().Split(",").ToList();
            //    foreach (string tagid in listTagid)
            //    {
            //        PTStatus PTStatus = new PTStatus(tagid);
            //        projectInfo.Status.Add(PTStatus);
            //    }

            //}

            if (string.IsNullOrWhiteSpace(Request.Form["hid_projectId"]))
            { // because the hid_projectId is empty, it will do insert here
                result = projectInfo.Save("");
            }
            else
            { // because there is already an Project id, it means we will override/change the existing details
                result = projectInfo.Save(Request.Form["hid_projectId"]);
            }

            if (result == "ok")
            {
                Response.Redirect("ProjectsLight");
            }
            else
            {
                errorMessage = result;
            }

            //Response.Redirect("Index");
        }
    }

}

