using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Collections.Generic;
using Telerik.Web.UI.PersistenceFramework;
using System.Web;

public partial class RegistersVesselList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //int CurrentPageIndex = 0;
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            //if (Session["DefaultCurrentPageIndex"] != null)
            //{
            //    int.TryParse(Convert.ToString(Session["DefaultCurrentPageIndex"]), out CurrentPageIndex);
            //}
            //gvVesselList.CurrentPageIndex = 0 ;

            if (Filter.CurrentVesselMasterListFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentVesselMasterListFilter;

                txtSearchVesselList.Text = nvc.Get("txtSearchVesselList").ToString();
                ddlActiveyn.SelectedValue = nvc.Get("ddlActiveyn").ToString();
                txtVesselCode.Text = nvc.Get("txtVesselCode").ToString();
                ucFlag.SelectedFlag = General.GetNullableInteger(nvc.Get("ucFlag").ToString()).ToString();
                ucVesselType.SelectedVesseltype = General.GetNullableInteger(nvc.Get("ucVesselType").ToString()).ToString();
                ucPrincipal.SelectedAddress = General.GetNullableInteger(nvc.Get("ucPrincipal").ToString()).ToString();
                ViewState["PAGENUMBER"] = General.GetNullableInteger(nvc.Get("PAGENUMBER").ToString());
                ViewState["SORTEXPRESSION"] = nvc.Get("SORTEXPRESSION").ToString();
                ViewState["SORTDIRECTION"] = General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString());
            }
            else
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();
                criteria.Add("txtSearchVesselList", txtSearchVesselList.Text);
                criteria.Add("ddlActiveyn", ddlActiveyn.SelectedValue);
                criteria.Add("txtVesselCode", txtVesselCode.Text);
                criteria.Add("ucFlag", "");
                criteria.Add("ucVesselType", "");
                criteria.Add("ucPrincipal", "");
                criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
                criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
                criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());

                Filter.CurrentVesselMasterListFilter = criteria;
            }

            toolbar.AddImageButton("../Registers/RegistersVesselList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvVesselList')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersVesselList.aspx", "Find", "search.png", "FIND");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbar.AddImageLink("../Registers/RegistersVessel.aspx?NewMode=true", "Add", "add.png", "VESSEL");
                toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','RegistersVesselChatbox.aspx?launchedFrom=VMLIST')", "Pending Issues", "pending-issue.png", "PENDINGISSUES");
            }

            MenuRegistersVesselList.AccessRights = this.ViewState;
            MenuRegistersVesselList.MenuList = toolbar.Show();
            ////MenuRegistersVesselList.SetTrigger(pnlVesselListEntry);
        }

        if (IsPostBack)
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();
            criteria.Add("txtSearchVesselList", txtSearchVesselList.Text);
            criteria.Add("ddlActiveyn", ddlActiveyn.SelectedValue);
            criteria.Add("txtVesselCode", txtVesselCode.Text);
            criteria.Add("ucFlag", ucFlag.SelectedFlag);
            criteria.Add("ucVesselType", ucVesselType.SelectedVesseltype);
            criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
            criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
            criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
            criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());

            Filter.CurrentVesselMasterListFilter = criteria;

            //gvVesselList.Rebind();
            //gvVesselList.CurrentPageIndex = gvVesselList.CurrentPageIndex+1;

        }
        if (Session["DefaultCurrentPageIndex"] != null)
        {
            this.gvVesselList.CurrentPageIndex = Convert.ToInt32(Session["RadGridCurrentPageIndex"]);
            this.gvVesselList.MasterTableView.Rebind();

        }

        //BindData();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELCODE", "FLDIMONUMBER", "FLDVESSELFLAG", "FLDVESSELTYPE" };
        string[] alCaptions = { "Vessel Name", "Vessel Code", "IMO Number", "Flag", "Vessel Type" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentVesselMasterListFilter;

        ds = PhoenixCommonRegisters.VesselSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , nvc.Get("txtSearchVesselList").ToString()
            , General.GetNullableString(nvc.Get("SORTEXPRESSION").ToString()),
            General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString()),
            gvVesselList.CurrentPageIndex,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(nvc.Get("ddlActiveyn").ToString()),
            General.GetNullableString(nvc.Get("txtVesselCode").ToString()),
            General.GetNullableInteger(nvc.Get("ucFlag").ToString()),
            General.GetNullableInteger(nvc.Get("ucVesselType").ToString()),
            General.GetNullableInteger(nvc.Get("ucPrincipal").ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvVesselList.Rebind();
            //BindData();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void gvVesselList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        //this.isIsNeedDataSource = true;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELCODE", "FLDIMONUMBER", "FLDVESSELFLAG", "FLDVESSELTYPE" };
        string[] alCaptions = { "Vessel Name", "Vessel Code", "IMO Number", "Flag", "Vessel Type" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentVesselMasterListFilter;

        gvVesselList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        //gvVesselList.CurrentPageIndex = 0;

        DataSet ds = PhoenixCommonRegisters.VesselSearch(
                                             PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                             ,  nvc.Get("txtSearchVesselList").ToString()
                                             ,  General.GetNullableString(nvc.Get("SORTEXPRESSION").ToString())
                                             ,  General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString())
                                             ,  gvVesselList.CurrentPageIndex+1
                                             ,  gvVesselList.PageSize
                                             ,  ref iRowCount
                                             ,  ref iTotalPageCount
                                             ,  General.GetNullableInteger(nvc.Get("ddlActiveyn").ToString()),
                                                General.GetNullableString(nvc.Get("txtVesselCode").ToString()),
                                                General.GetNullableInteger(nvc.Get("ucFlag").ToString()),
                                                General.GetNullableInteger(nvc.Get("ucVesselType").ToString()),
                                                General.GetNullableInteger(nvc.Get("ucPrincipal").ToString()));

        //General.SetPrintOptions("gvVesselList", "Vessel List", alCaptions, alColumns, ds);
        gvVesselList.DataSource = ds;
        gvVesselList.VirtualItemCount = iRowCount;

        if (gvVesselList.CurrentPageIndex + 1 < int.Parse(ViewState["PAGENUMBER"].ToString()))
        {
            ViewState["PAGENUMBER"] = gvVesselList.CurrentPageIndex + 1;
        }
        else
        {
            ViewState["PAGENUMBER"] = int.Parse(ViewState["PAGENUMBER"].ToString()) + 1;
        }

        

        //gvVesselList.MasterTableView.Items[0].Selected = true;
        //Filter.CurrentVesselMasterFilter= gvVesselList.Items[0].ItemIndex.ToString();


    }
    protected void gvVesselList_PreRender(object sender, EventArgs e)
    {
        //if(Filter.CurrentVesselMasterFilter!=null)
        //{
        //    gvVesselList.MasterTableView.s = Filter.CurrentVesselMasterFilter;

        //}
    }
    protected void gvVesselList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "EDIT")
        {
            GridDataItem item = (GridDataItem)e.Item;

            string vesselid = item.GetDataKeyValue("FLDVESSELID").ToString();

            Filter.CurrentVesselMasterFilter = vesselid;
            Response.Redirect("../Registers/RegistersVessel.aspx?Vesselid=" + Filter.CurrentVesselMasterListFilter);
        }

        if(e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            ShowExcel();
        }
        if(e.CommandName=="Page")
        {
            ViewState["PAGENUMBER"] = gvVesselList.CurrentPageIndex;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvVesselList_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        //ViewState["PAGENUMBER"] = e.NewPageIndex;
    }

    //public int SavedPageIndex
    //{
    //    get
    //    {
    //        if (ViewState["PAGENUMBER"] == null)
    //            ViewState["PAGENUMBER"] = 0;
    //        return (int)ViewState["PAGENUMBER"];
    //    }
    //    set { ViewState["PAGENUMBER"] = value; }
    //}

    public class SessionStorageProvider : IStateStorageProvider
    {
        private System.Web.SessionState.HttpSessionState session = HttpContext.Current.Session;
        static string storageKey;

        public static string StorageProviderKey
        {
            set { storageKey = value; }
        }

        public void SaveStateToStorage(string key, string serializedState)
        {
            session[storageKey] = serializedState;
        }

        public string LoadStateFromStorage(string key)
        {
            return session[storageKey].ToString();
        }
    }
}
