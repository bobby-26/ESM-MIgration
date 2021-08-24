using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Common_CommonPickListInistituteList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuInstitute.MenuList = toolbarmain.Show();
       
        if (!IsPostBack)
        {
            ViewState["DOCUMENTID"] = "";
            ViewState["selectednode"] = "";
            ViewState["TYPE"] = "0";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvInstitute.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string instituteName = null, instituteShortCode = null;
        instituteName = txtInstituteNameSearch.Text;
        instituteShortCode = txtInstituteShortCodeSearch.Text;
        //string[] alColumns = { "S.NO", "FLDLOCATION" };
        //string[] alCaptions = { "S.No", "Available Location" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewInstitute.CrewInstituteSearch(instituteName
                                                             , instituteShortCode
                                                             , sortexpression
                                                             , sortdirection
                                                             , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                             , gvInstitute.PageSize
                                                             , ref iRowCount
                                                             , ref iTotalPageCount);
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvInstitute.DataSource = ds;
            gvInstitute.VirtualItemCount = iRowCount;
        }
        else
        {
            gvInstitute.DataSource = "";           
        }
    }
    protected void MenuInstitute_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInstitute_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string Script = "";
            NameValueCollection nvc;

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";


            nvc = Filter.CurrentPickListSelection;
            RadLabel lblInstituteId = (RadLabel)e.Item.FindControl("lblInstituteId");
            nvc.Set(nvc.GetKey(1), lblInstituteId.Text);

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkInstituteName");
            nvc.Set(nvc.GetKey(2), lb.Text.ToString());

            Filter.CurrentPickListSelection = nvc;

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvInstitute_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInstitute.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInstitute_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }
}
