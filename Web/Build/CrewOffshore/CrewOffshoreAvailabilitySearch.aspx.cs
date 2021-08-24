using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewOffshoreAvailabilitySearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
            toolbarsub.AddButton("Search", "SEARCH",ToolBarDirection.Right);
            
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();
            ViewState["pl"] = "";
            if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                ViewState["pl"] = Request.QueryString["pl"].ToString();

            if (!IsPostBack)
            {
                DateTime now = DateTime.Now;
                DateTime lastonemonth = now.AddDays(-30);
                DateTime nexttwomonths = now.AddDays(60);
                txtDOAFrom.Text = lastonemonth.ToString("dd/MM/yyyy");
                txtDOATo.Text = nexttwomonths.ToString("dd/MM/yyyy");
                txtcontactFrom.Text = lastonemonth.ToString("dd/MM/yyyy");
                Bindstatus();
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Bindstatus()
    {
        DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , 54, 1, "APR,ONL");
        ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , 53, 1, "LFT").Tables[0]);

        ddlstatus.DataSource = ds;

        ddlstatus.DataValueField = "FLDHARDCODE";
        ddlstatus.DataTextField = "FLDHARDNAME";
       
        //ddlstatus.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        ddlstatus.DataBind();
        ddlstatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void Clear()
    {
        txtName.Text = "";
        txtFileNo.Text = "";
        ddlRank.SelectedRank = "";
        txtcontactto.Text = "";
        ddlNationality.SelectedNationality = "";
        txtDOAFrom.Text = "";
        txtDOATo.Text = "";
        txtcontactFrom.Text = "";
       
        ddlstatus.SelectedIndex = -1;
        ddlstatus.ClearSelection();
        ddlstatus.Text = string.Empty;


    }
    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("txtName", txtName.Text);
            nvc.Add("txtFileNo", txtFileNo.Text);
            nvc.Add("ddlRank", ddlRank.SelectedRank);
            nvc.Add("txtDOAFrom", txtDOAFrom.Text);
            nvc.Add("txtDOATo", txtDOATo.Text);
            nvc.Add("txtContactFrom", txtcontactFrom.Text);
            nvc.Add("txtContactTo", txtcontactto.Text);
            nvc.Add("chkincludentbr", chkincludentbr.Checked ? "1" : "0");
            nvc.Add("ddlstatus", ddlstatus.SelectedValue);
            nvc.Add("ddlNationality", ddlNationality.SelectedNationality);
            Filter.CurrentOffshoreAvailabilitySearch = nvc;
            Response.Redirect("CrewOffshoreAvailability.aspx?pl="+ViewState["pl"].ToString(), true);
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Clear();
        }
    }
}
