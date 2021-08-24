using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using Telerik.Web.UI;
public partial class CrewFamilyNokFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO" ,ToolBarDirection.Right);
        
        MenuActivityFilterMain.AccessRights = this.ViewState;
        MenuActivityFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            btnSubmit.Attributes.Add("style", "display:none");                       
            ddlInActive.SelectedValue = "1";
            rblInActive_SelectedIndex(null, null);
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {

        NameValueCollection nvc = Filter.CurrentNewApplicantFilterCriteria;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        if (nvc != null && !IsPostBack)
        {
            txtName.Text = (nvc.Get("txtName") == null) ? "" : nvc.Get("txtName").ToString();
            txtFileNumber.Text = (nvc.Get("txtFileNumber") == null) ? "" : nvc.Get("txtFileNumber").ToString();           
            txtPassortNo.Text = (nvc.Get("txtPassortNo") == null) ? "" : nvc.Get("txtPassortNo").ToString();
            txtSeamanbookNo.Text = (nvc.Get("txtSeamanbookNo") == null) ? "" : nvc.Get("txtSeamanbookNo").ToString();
            txtNOKName.Text = (nvc.Get("txtNOKName") == null) ? "" : nvc.Get("txtNOKName").ToString();            
            if ((nvc.Get("ddlInActive") != "Dummy") && (nvc.Get("ddlInActive") != null))
                ddlInActive.SelectedValue = nvc.Get("ddlInActive").ToString();
            else
                ddlInActive.SelectedValue = "1";
            
            ddlZone.SelectedZone = (General.GetNullableInteger(nvc.Get("ddlZone")) == null) ? "" : nvc.Get("ddlZone").ToString();            
            lstNationality.NationalityList = PhoenixRegistersCountry.ListNationality();
            lstNationality.SelectedList = (nvc.Get("lstNationality") == null) ? "" : nvc.Get("lstNationality").ToString();
            lstRank.RankList = PhoenixRegistersRank.ListRank();
            lstRank.selectedlist = (nvc.Get("lstRank") == null) ? "" : nvc.Get("lstRank").ToString();
            if ((nvc.Get("ucPrincipal") != "Dummy") && (nvc.Get("ucPrincipal") != null))
                ucPrincipal.SelectedAddress = nvc.Get("ucPrincipal").ToString();
            else
            {
                ucPrincipal.SelectedAddress = "";
            }
            PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 54, 1, "NWA,OBP,OLP,ONB");
            if (nvc.Get("ddlStatus") != null)
            {
                string strlist = "," + nvc.Get("ddlStatus").ToString() + ",";
                foreach (RadListBoxItem item in lstStatus.Items)
                {
                    item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
                }
            }
            else
            {
                lstStatus.SelectedValue = null;
            }
        }
    }
    protected void rblInActive_SelectedIndex(object sender, EventArgs e)
    {
        if (ddlInActive.SelectedValue == "1")
        {
            lstStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 54, 1, string.Empty);
            lstStatus.DataBind();
        }
        else if (ddlInActive.SelectedValue == "0")
        {
            lstStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 53, 1, "MDL,LSL,NTB,LFT,DTH,DST,EXM,TSP");
            lstStatus.DataBind();
        }
        else
        {
            DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , 54, 1, string.Empty);
            ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , 53, 1, "MDL,LSL,NTB,LFT,DTH,DST,EXM,TSP").Tables[0]);

            lstStatus.DataSource = ds;
            lstStatus.DataBind();
        }
    }
    private string StatusSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstStatus.Items)
        {
            if (item.Selected == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        return strlist.ToString().TrimEnd(',');
    }

    protected void BindClear()
    {
        txtName.Text = string.Empty;
        txtFileNumber.Text = string.Empty;
        txtPassortNo.Text = string.Empty;
        txtSeamanbookNo.Text = string.Empty;       
        lstRank.selectedlist = string.Empty;
        lstNationality.SelectedList = string.Empty;        
        ucPrincipal.SelectedAddress = string.Empty;
        ddlInActive.SelectedValue = string.Empty;
        txtNOKName.Text = string.Empty;
        Filter.CurrentNewApplicantFilterCriteria = null;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";


        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();

        criteria.Add("txtName", txtName.Text);
        criteria.Add("txtFileNumber", txtFileNumber.Text);
        criteria.Add("txtRefNumber", string.Empty);
        criteria.Add("txtPassortNo", txtPassortNo.Text);
        criteria.Add("txtSeamanbookNo", txtSeamanbookNo.Text);       
        criteria.Add("lstRank", lstRank.selectedlist);
        criteria.Add("lstNationality", lstNationality.SelectedList);        
        criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
        criteria.Add("ddlActiveYN", ddlInActive.SelectedValue);
        criteria.Add("ddlStatus", StatusSelectedList());
        criteria.Add("txtNOKName", txtNOKName.Text);

        Filter.CurrentNewApplicantFilterCriteria = criteria;

        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

    }

    protected void NewApplicantFilterMain_TabStripCommand(object sender, EventArgs e)
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

            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtFileNumber", txtFileNumber.Text);
            criteria.Add("txtRefNumber", string.Empty);
            criteria.Add("txtPassortNo", txtPassortNo.Text);
            criteria.Add("txtSeamanbookNo", txtSeamanbookNo.Text);           
            criteria.Add("lstRank", lstRank.selectedlist);
            criteria.Add("lstNationality", lstNationality.SelectedList);            
            criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
            criteria.Add("ddlActiveYN", ddlInActive.SelectedValue);
            criteria.Add("ddlStatus", StatusSelectedList());
            criteria.Add("txtNOKName", txtNOKName.Text);

            Filter.CurrentNewApplicantFilterCriteria = criteria;

            //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

        }

        if (CommandName.ToUpper().Equals("CLEAR"))
        {

            BindClear();

        }
        if (CommandName.ToUpper().Equals("CANCEL"))
        {
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
    }
 
}
