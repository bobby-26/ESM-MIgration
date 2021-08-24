using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;

public partial class Inspection_InspectionPSCMOUEmailConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }
                gvEmailConfiguration.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);             
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPSCMOUEmailConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvEmailConfiguration')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuEmailConfiguration.AccessRights = this.ViewState;
            MenuEmailConfiguration.MenuList = toolbar.Show();

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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDDAYSPRIOR" };
        string[] alCaptions = { "Vessel", "Days (Prior from ETA)" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionPSCMOUMatrix.ListEmailDaysConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(ucVessel.SelectedVessel),            
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvEmailConfiguration.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=EmailDaysConfig.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Client BPG Comments</h3></td>");
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

    protected void EmailConfiguration_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvEmailConfiguration.SelectedIndexes.Clear();
        gvEmailConfiguration.EditIndexes.Clear();
        gvEmailConfiguration.DataSource = null;
        gvEmailConfiguration.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDVESSELNAME", "FLDDAYSPRIOR" };
        string[] alCaptions = { "Vessel", "Days (Prior from ETA)" };

        DataSet ds = PhoenixInspectionPSCMOUMatrix.ListEmailDaysConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(ucVessel.SelectedVessel),            
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvEmailConfiguration.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvEmailConfiguration", "Client BPG Comments", alCaptions, alColumns, ds);

        gvEmailConfiguration.DataSource = ds;
        gvEmailConfiguration.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvEmailConfiguration_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDepartment(((RadTextBox)e.Item.FindControl("txtDaysAdd")).Text
                    , ((UserControlVesselByOwner)e.Item.FindControl("ucAddVessel")).SelectedVessel))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertComments(
                    (((UserControlVesselByOwner)e.Item.FindControl("ucAddVessel")).SelectedVessel.ToString()),
                    ((RadTextBox)e.Item.FindControl("txtDaysAdd")).Text

                );
                Rebind();
               // ((RadTextBox)e.Item.FindControl("txtCommentsAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDepartment(((RadTextBox)e.Item.FindControl("txtDaysEdit")).Text
                    , ((RadLabel)e.Item.FindControl("lblvesselid")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                UpdateComments(
                     ((RadLabel)e.Item.FindControl("lblDaysEdit")).Text,
                     ((RadLabel)e.Item.FindControl("lblvesselid")).Text,
                     ((RadTextBox)e.Item.FindControl("txtDaysEdit")).Text                     
                 );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteComments(((RadLabel)e.Item.FindControl("lblNoticedid")).Text);
                Rebind();
            }

            else if (e.CommandName == "Page")
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
    protected void gvEmailConfiguration_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);


            //RadComboBox ddlEditCompany = (RadComboBox)e.Item.FindControl("ddlEditCompany");
            //DataRowView drvcompany = (DataRowView)e.Item.DataItem;
            //if (ddlEditCompany != null)
            //{
            //    DataSet ds = PhoenixInspectionOilMajorComany.ListOilMajorCompany(1, null);
            //    ddlEditCompany.DataSource = ds;
            //    ddlEditCompany.DataBind();
            //    ddlEditCompany.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            //    ddlEditCompany.SelectedValue = drvcompany["FLDCOMPANY"].ToString();
            //}
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            UserControlVesselByOwner vsl = ((UserControlVesselByOwner)e.Item.FindControl("ucAddVessel"));


            NameValueCollection nv = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;

            if (nv.Get("QMS") != null && nv.Get("QMS") != "")
            {
                vsl.Company = nv.Get("QMS");
                vsl.bind();
            }
                //vsl.VesselList = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, 16, null, 1);


        }
    }
    private void InsertComments(string vessel, string days)
    {
        PhoenixInspectionPSCMOUMatrix.EmailDaysConfigurationInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    General.GetNullableInteger(vessel),
                                    General.GetNullableInteger(days));
        ucStatus.Text = "Days configured successfully";
    }

    private void UpdateComments(string notificationid, string vessel, string days)
    {
        PhoenixInspectionPSCMOUMatrix.EmailDaysConfigurationUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                General.GetNullableGuid(notificationid),
                                General.GetNullableInteger(vessel),
                                General.GetNullableInteger(days)
                                    );
        ucStatus.Text = "Days updated successfully";
    }

    private bool IsValidDepartment(string days, string vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvEmailConfiguration;

        if (days.Trim().Equals(""))
            ucError.ErrorMessage = "Days is required.";

        if (string.IsNullOrEmpty(vessel))
            ucError.ErrorMessage = "Vessel  is required.";

        return (!ucError.IsError);
    }

    private void DeleteComments(string notificationid)
    {
        PhoenixInspectionPSCMOUMatrix.EmailDaysConfigurationDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(notificationid));
    }

    protected void gvEmailConfiguration_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEmailConfiguration.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEmailConfiguration_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        gvEmailConfiguration.Rebind();
    }
}