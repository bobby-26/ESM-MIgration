using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewRecommendedCourseFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        //toolbar.AddButton("Cancel", "CANCEL");
        coursefilter.AccessRights = this.ViewState;
        coursefilter.MenuList = toolbar.Show();
    }
    protected void coursefilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";
        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();

            criteria.Add("txtcourseCode", txtCourseCode.Text);
            criteria.Add("txtcourseName", txtcourseName.Text);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);

            Filter.CurrentRecommendedCourseFilter = criteria;
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

        }
        //if (CommandName.ToUpper().Equals("GO"))
        //{
        //    Response.Redirect("../Crew/CrewTrainingSchedule.aspx", true);
        //}

       
    }
}