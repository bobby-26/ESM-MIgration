using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class DryDockDiscussion : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();        
        toolbarmain.AddButton("Post Remarks", "POSTCOMMENT",ToolBarDirection.Right);
        MenuDiscussion.AccessRights = this.ViewState;
        MenuDiscussion.MenuList = toolbarmain.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("List", "LIST");
        toolbar.AddButton("Details", "DETAIL");
        toolbar.AddButton("Supt Remarks", "SUPTREMARKS");
        toolbar.AddButton("Work Requests", "WORKREQUESTS");
        toolbar.AddButton("Attachment", "ATTACHMENT");




        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbar.Show();
        MenuHeader.SelectedMenuIndex = 2;

        if (!IsPostBack)
        {
           // lblUsername.Text = PhoenixSecurityContext.CurrentSecurityContext.UserName;
          //  lblCurrentdate.Text = System.DateTime.Today.ToString("dd/MM/yyyy");
            ViewState["DTKEY"] = Request.QueryString["DTKEY"];
            ViewState["JOBID"] = Request.QueryString["REPAIRJOBID"] != null ? Request.QueryString["REPAIRJOBID"] : Request.QueryString["STANDARDJOBID"];            
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            repDiscussion.PageSize = 100;
            BindData();
        }
       
    }
  
    private void BindData()
    {
        
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixDryDockDiscussion.DryDockDiscussionSearch(General.GetNullableGuid(ViewState["DTKEY"].ToString()), 1
           , PhoenixSecurityContext.CurrentSecurityContext.VesselID
           , sortdirection
           , (int)ViewState["PAGENUMBER"]
           , General.ShowRecords(null)
           , ref iRowCount, ref iTotalPageCount);

        repDiscussion.DataSource = dt;
        //repDiscussion.DataBind();
        repDiscussion.VirtualItemCount = iRowCount;




        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

      
    }

    private void ShowNoRecordsFound(DataTable dt, Repeater  rep)
    {
        dt.Rows.Add(dt.NewRow());
        rep.DataSource = dt;
        rep.DataBind();
    }

    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (Request.QueryString["REPAIRJOBID"] != null)
        {
            if (CommandName.ToUpper().Equals("DETAIL"))
            {
                Response.Redirect("../DryDock/DryDockJob.aspx?REPAIRJOBID=" + ViewState["JOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            }
            else if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../DryDock/DryDockJobList.aspx?REPAIRJOBID=" + ViewState["JOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                Response.Redirect("../DryDock/DryDockJobAttachments.aspx?REPAIRJOBID=" + ViewState["JOBID"].ToString() + "&DTKEY=" + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + Request.QueryString["pno"], false);
            }           
            else if (CommandName.ToUpper().Equals("WORKREQUESTS"))
            {
                Response.Redirect("../DryDock/DryDockStandardJobWorkRequest.aspx?REPAIRJOBID=" + ViewState["JOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("DETAIL"))
            {
                Response.Redirect("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + ViewState["JOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            }
            else if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../DryDock/DryDockJobGeneralList.aspx?STANDARDJOBID=" + ViewState["JOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                Response.Redirect("../DryDock/DryDockJobAttachments.aspx?STANDARDJOBID=" + ViewState["JOBID"].ToString() + "&DTKEY=" + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + Request.QueryString["pno"], false);
            }
            else if (CommandName.ToUpper().Equals("WORKREQUESTS"))
            {
                Response.Redirect("../DryDock/DryDockStandardJobWorkRequest.aspx?STANDARDJOBID=" + ViewState["JOBID"].ToString() + "&pno=" + Request.QueryString["pno"]);
            }           
        }
    }

    protected void MenuDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("POSTCOMMENT"))
            {
                if (!IsCommentValid(txtNotesDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDryDockDiscussion.UpdateDryDockDiscussion(General.GetNullableGuid(ViewState["DTKEY"].ToString()).Value, 1
                    , General.GetNullableGuid(ViewState["JOBID"].ToString())
                    , txtNotesDescription.Text
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                BindData();
                txtNotesDescription.Text = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsCommentValid(string strComment)
    {
        ucError.HeaderMessage = "Please provide the following required information";
       
        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);

    }
 

  
    
   

  
}
