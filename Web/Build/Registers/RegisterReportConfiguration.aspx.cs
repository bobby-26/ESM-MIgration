using System;
using System.Collections;
using System.Configuration;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Registers_RegisterReportConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            //toolbar.AddFontAwesomeButton("../Registers/RegisterReportConfiguration.aspx", "Export to Excel","<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("../Registers/RegisterReportConfiguration.aspx", "Filter","<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvreportconfiguration')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersConfig.AccessRights = this.ViewState;
            MenuRegistersConfig.MenuList = toolbar.Show();
          
            // BindMapping();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["CURRENTINDEX"] = 1;
                gvreportconfiguration.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersConfig_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //if (CommandName.ToUpper().Equals("EXCEL"))
            //{
            //    ShowExcel();
            //}
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvreportconfiguration.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDOCUMENTNAME", "FLDDISPLAYNAME", "FLDDATATODISPLAY", "FLDORDER" };
        string[] alCaptions = { "DocumentId", "Display Name", "Data to dislay" ,"Order" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegisterReportConfiguration.ReportConfigurationSearch(General.GetNullableInteger(ddlreporttype.SelectedValue.ToString())
                                                                                , General.GetNullableInteger(ddldocumenttype.SelectedValue.ToString())
                                                                                , (int)ViewState["PAGENUMBER"]
                                                                                , gvreportconfiguration.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ReportConfiguration.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Report Configuration</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
              
        string[] alColumns = { "FLDDOCUMENTNAME", "FLDDISPLAYNAME", "FLDDATATODISPLAY", "FLDORDER" };
        string[] alCaptions = { "DocumentId", "Display Name", "Data to dislay", "Order" };

        DataTable dt = PhoenixRegisterReportConfiguration.ReportConfigurationSearch(General.GetNullableInteger(ddlreporttype.SelectedValue.ToString())
                                                                                , General.GetNullableInteger(ddldocumenttype.SelectedValue.ToString())
                                                                                , (int)ViewState["PAGENUMBER"]
                                                                                , gvreportconfiguration.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvreportconfiguration", "Course", alCaptions, alColumns, ds);

        gvreportconfiguration.DataSource = ds;
        gvreportconfiguration.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

   
    protected void gvreportconfiguration_ItemDataBound(object sender, GridItemEventArgs e)
    {   
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }
            RadComboBox ddldatadiaplayedit = (RadComboBox)e.Item.FindControl("ddldatadiaplayedit");
            if(ddldatadiaplayedit!=null)
            {
                RadLabel lnkdatadisplayidedit = (RadLabel)e.Item.FindControl("lnkdatadisplayidedit");
                ddldatadiaplayedit.SelectedValue = lnkdatadisplayidedit.Text;
            }

       }
        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            RadComboBox DDLDOCUMENTADD = (RadComboBox)e.Item.FindControl("ddldocumentadd");
            if (DDLDOCUMENTADD != null)
            {
                DataTable dt = PhoenixRegisterReportConfiguration.ReportConfigureDocumentList(General.GetNullableInteger(ddldocumenttype.SelectedValue.ToString()));
                if (dt.Rows.Count > 0)
                {
                    DDLDOCUMENTADD.Items.Clear();
                    DDLDOCUMENTADD.DataSource = dt;
                    DDLDOCUMENTADD.DataTextField = "FLDDOCUMENTNAME";
                    DDLDOCUMENTADD.DataValueField = "FLDDOCUMENTID";
                    DDLDOCUMENTADD.DataBind();

                }
                DDLDOCUMENTADD.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
}
        }
    }

    protected void gvreportconfiguration_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadComboBox ddldocumentadd = (RadComboBox)e.Item.FindControl("ddldocumentadd");
                RadTextBox txtdisplaynameadd = (RadTextBox)e.Item.FindControl("txtdisplaynameadd");
                RadComboBox ddldatadiaplayadd = (RadComboBox)e.Item.FindControl("ddldatadiaplayadd");
                UserControlMaskNumber txtorderadd = (UserControlMaskNumber)e.Item.FindControl("txtorderadd");

                PhoenixRegisterReportConfiguration.ReportConfigureInsert(null, General.GetNullableInteger(ddlreporttype.SelectedValue.ToString())
                                                                          , General.GetNullableInteger(ddldocumenttype.SelectedValue.ToString())
                                                                          , General.GetNullableInteger(ddldocumentadd.SelectedValue.ToString())
                                                                          , txtdisplaynameadd.Text
                                                                          , General.GetNullableInteger(ddldatadiaplayadd.SelectedValue.ToString())
                                                                          , General.GetNullableInteger(txtorderadd.Text));
                BindData();
                gvreportconfiguration.Rebind();
                // ((DropDownList)_gridView.FooterRow.FindControl("ddlRankGroupAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = e.Item as GridDataItem;

                string configurationid = item.GetDataKeyValue("FLDREPORTCONFIGID").ToString();
                PhoenixRegisterReportConfiguration.ReportConfigureDelete(General.GetNullableGuid(configurationid));
                BindData();
                gvreportconfiguration.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                string configurationid = item.GetDataKeyValue("FLDREPORTCONFIGID").ToString();
                // DropDownList ddldocumentadd = (DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddldocumentedit");
                RadTextBox txtdisplaynameedit = (RadTextBox)e.Item.FindControl("txtdisplaynameedit");
                RadComboBox ddldatadiaplayedit = (RadComboBox)e.Item.FindControl("ddldatadiaplayedit");
                UserControlMaskNumber txtorderedit = (UserControlMaskNumber)e.Item.FindControl("txtorderedit");
                RadLabel lbldocumentidedit = (RadLabel)e.Item.FindControl("lbldocumentidedit");
                RadLabel lnkdoctypenumedit = (RadLabel)e.Item.FindControl("lnkdoctypenumedit");

                PhoenixRegisterReportConfiguration.ReportConfigureInsert(General.GetNullableGuid(configurationid)
                                                                          , General.GetNullableInteger(ddlreporttype.SelectedValue.ToString())
                                                                          , General.GetNullableInteger(lnkdoctypenumedit.Text)
                                                                          , General.GetNullableInteger(lbldocumentidedit.Text)
                                                                          , txtdisplaynameedit.Text
                                                                          , General.GetNullableInteger(ddldatadiaplayedit.SelectedValue.ToString())
                                                                          , General.GetNullableInteger(txtorderedit.Text));
                BindData();
                gvreportconfiguration.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
            
            }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvreportconfiguration_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvreportconfiguration.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvreportconfiguration_Sorting(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

}
