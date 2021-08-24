using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;
public partial class CrewOffshoreReimbursementFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
       
        MenuPD.AccessRights = this.ViewState;
        MenuPD.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
           
            BindPurpose();
        }
    }

    protected void PD_TabStripCommand(object sender, EventArgs e)
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

            criteria.Add("ddlRank", ddlRank.SelectedRank);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtFileNo", txtFileNo.Text);
            criteria.Add("chkActive", chkActive.Checked ? "1" : "0");
            criteria.Add("ddlEarDed", ddlEarDed.SelectedValue);
            criteria.Add("ddlPurpose", ddlPurpose.SelectedValue);

            Filter.CrewOffshoreReimbursementFilterSelection = criteria;
        }

        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                       "BookMarkScript", "parent.fnReloadList('Filter', 'ifMoreInfo', null);", true);
    }

    protected void ddlEarDed_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindPurpose();
    }

    protected void BindPurpose()
    {
        DataSet ds = new DataSet();
        if (General.GetNullableInteger(ddlEarDed.SelectedValue) != null)
        {
            if (General.GetNullableInteger(ddlEarDed.SelectedValue).HasValue && General.GetNullableInteger(ddlEarDed.SelectedValue).Value < 0)
            {
                ds = PhoenixRegistersHard.ListHard(1, 129, 0, "CRR,ADL,TFR");
            }
            else
            {
                ds = PhoenixRegistersHard.ListHard(1, 128, 0, "TRV,USV,AFR,EBG,CFE,LFE,MEF");
            }
            ddlPurpose.Items.Clear();
            ddlPurpose.DataSource = ds;
            ddlPurpose.DataTextField = "FLDHARDNAME";
            ddlPurpose.DataValueField = "FLDHARDCODE";
            ddlPurpose.DataBind();
            ddlPurpose.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
        else
        {
            ddlPurpose.Items.Clear();
            ddlPurpose.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
    }
}
