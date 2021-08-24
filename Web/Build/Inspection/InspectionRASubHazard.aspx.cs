using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRASubHazard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRASubHazard.aspx?" + Request.QueryString["type"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRASubHazard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRASubHazard.aspx?type=" + Request.QueryString["type"], "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Copy','','" + Session["sitepath"] + "/Inspection/InspectionRAHazardMachineryDamageMapping.aspx',null);return true;", "Map to Machinery Damage/ Failure", "<i class=\"fas fa-tasks\"></i>", "MAPTOMACHINERYDAMAGE");

            MenuRASubHazard.AccessRights = this.ViewState;
            MenuRASubHazard.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvRASubHazard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["TYPE"] = Request.QueryString["type"];
                ucHazard.Type = Request.QueryString["type"];
                if (ViewState["TYPE"].ToString() == "1")
                {
                    ucTitle.Text = "Health and Safety Impact";
                }
                else if (ViewState["TYPE"].ToString() == "2")
                {
                    ucTitle.Text = "Environmental Impact";
                }
                else if (ViewState["TYPE"].ToString() == "4")
                {
                    ucTitle.Text = "Economic Impact";
                }
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSUBHAZARDNAME", "FLDHAZARDNAME", "FLDSEVERITY", "FLDSCORE", "FLDCATEGORY" };
        string[] alCaptions = { "Sub Hazard", "Hazard", "Severity", "Score", "Consequence Category" };


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentSubHazard.RiskAssessmentSubHazardSearch(
            Convert.ToInt32(ViewState["TYPE"].ToString()),
            General.GetNullableInteger(ucHazard.SelectedHazardType),
            General.GetNullableString(txtName.Text),
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SubHazard.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + ucTitle.Text + "</h3></td>");
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

    protected void RASubHazard_TabStripCommand(object sender, EventArgs e)
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSUBHAZARDNAME", "FLDHAZARDNAME", "FLDSEVERITY", "FLDSCORE", "FLDCATEGORY" };
        string[] alCaptions = { "Sub Hazard", "Hazard", "Severity", "Score", "Consequence Category" };

        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentSubHazard.RiskAssessmentSubHazardSearch(
            Convert.ToInt32(ViewState["TYPE"].ToString()),
            General.GetNullableInteger(ucHazard.SelectedHazardType),
            General.GetNullableString(txtName.Text),
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvRASubHazard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvRASubHazard", ucTitle.Text, alCaptions, alColumns, ds);

        gvRASubHazard.DataSource = ds;
        gvRASubHazard.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void Rebind()
    {
        gvRASubHazard.SelectedIndexes.Clear();
        gvRASubHazard.EditIndexes.Clear();
        gvRASubHazard.DataSource = null;
        gvRASubHazard.Rebind();
    }

    protected void gvRASubHazard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidRASubHazard(((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                         ((UserControlRAHazardType)e.Item.FindControl("ucHazardAdd")).SelectedHazardType,
                         ((UserControlRASeverity)e.Item.FindControl("ucSeverityAdd")).SelectedSeverity))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertRASubHazard(
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                    ((UserControlRAHazardType)e.Item.FindControl("ucHazardAdd")).SelectedHazardType,
                    ((UserControlRASeverity)e.Item.FindControl("ucSeverityAdd")).SelectedSeverity,
                    ((RadTextBox)e.Item.FindControl("txtCategoryAdd")).Text
                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidRASubHazard(((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                        ((UserControlRAHazardType)e.Item.FindControl("ucHazardEdit")).SelectedHazardType,
                         ((UserControlRASeverity)e.Item.FindControl("ucSeverityEdit")).SelectedSeverity))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateRASubHazard(
                        (((RadLabel)e.Item.FindControl("lblSubHazardIdEdit")).Text),
                         ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                         ((UserControlRAHazardType)e.Item.FindControl("ucHazardEdit")).SelectedHazardType,
                         ((UserControlRASeverity)e.Item.FindControl("ucSeverityEdit")).SelectedSeverity,
                         ((RadTextBox)e.Item.FindControl("txtCategoryEdit")).Text
                     );
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteSubHazard(new Guid(((RadLabel)e.Item.FindControl("lblSubHazardId")).Text));
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("TYPEMAPPING"))
            {
                string subhazardid = ((RadLabel)e.Item.FindControl("lblSubHazardId")).Text;
                Response.Redirect("../Inspection/InspectionRAImpactDocumentPPEMapping.aspx?subhazardid=" + subhazardid);
            }
            if (e.CommandName == "Page")
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

    //protected void gvRASubHazard_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        if (!IsValidRASubHazard(((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
    //                    ((UserControlRAHazardType)e.Item.FindControl("ucHazardEdit")).SelectedHazardType,
    //                     ((UserControlRASeverity)e.Item.FindControl("ucSeverityEdit")).SelectedSeverity))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        UpdateRASubHazard(
    //                (((RadLabel)e.Item.FindControl("lblSubHazardIdEdit")).Text),
    //                 ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
    //                 ((UserControlRAHazardType)e.Item.FindControl("ucHazardEdit")).SelectedHazardType,
    //                 ((UserControlRASeverity)e.Item.FindControl("ucSeverityEdit")).SelectedSeverity,
    //                 ((RadTextBox)e.Item.FindControl("txtCategoryEdit")).Text

    //             );
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void gvRASubHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            var ddl = e.Item.FindControl("ucHazardEdit") as UserControlRAHazardType;
            if (ddl != null)
            {
                ddl.Type = Request.QueryString["type"];
                ddl.HazardTypeList = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(Convert.ToInt32(Request.QueryString["type"]), 0);

            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlRAHazardType ucHazard = (UserControlRAHazardType)e.Item.FindControl("ucHazardEdit");
            DataRowView drvHazardType = (DataRowView)e.Item.DataItem;
            if (ucHazard != null) ucHazard.SelectedHazardType = drvHazardType["FLDHAZARDID"].ToString();

            UserControlRASeverity ucSeverity = (UserControlRASeverity)e.Item.FindControl("ucSeverityEdit");
            DataRowView drvSeverity = (DataRowView)e.Item.DataItem;
            if (ucSeverity != null) ucSeverity.SelectedSeverity = drvSeverity["FLDSEVERITYID"].ToString();

            //LinkButton cmdMap = (LinkButton)e.Row.FindControl("cmdTypeMapping");
            //if (cmdMap != null) cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);

            //RadLabel lblSubHazardId = (RadLabel)e.Row.FindControl("lblSubHazardId");

            //if (cmdMap != null)
            //{
            //    cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);
            //    cmdMap.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../Inspection/InspectionRAImpactDocumentPPEMapping.aspx?subhazardid=" + lblSubHazardId.Text.ToString() + "');return false;");
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
            var ddl = e.Item.FindControl("ucHazardAdd") as UserControlRAHazardType;
            if (ddl != null)
            {
                ddl.Type = Request.QueryString["type"];
                ddl.HazardTypeList = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(Convert.ToInt32(Request.QueryString["type"]), 0);

            }
        }
    }

    private void InsertRASubHazard(string name, string Hazardid, string severityid, string category)
    {

        PhoenixInspectionRiskAssessmentSubHazard.InsertRiskAssessmentSubHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              Convert.ToInt32(Hazardid), name, Convert.ToInt32(severityid), General.GetNullableString(category));
    }

    private void UpdateRASubHazard(string SubHazardid, string name, string Hazardid, string severityid, string category)
    {

        PhoenixInspectionRiskAssessmentSubHazard.UpdateRiskAssessmentSubHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(SubHazardid), Convert.ToInt32(Hazardid), name, Convert.ToInt32(severityid), General.GetNullableString(category));
        ucStatus.Text = "Information updated";
    }

    private bool IsValidRASubHazard(string name, string Hazard, string severityid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvRASubHazard;

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Impact is required.";

        if (General.GetNullableInteger(Hazard) == null)
            ucError.ErrorMessage = "Hazard is required.";

        if (General.GetNullableInteger(severityid) == null)
            ucError.ErrorMessage = "Severity is required.";

        return (!ucError.IsError);
    }

    private void DeleteSubHazard(Guid SubHazardid)
    {
        PhoenixInspectionRiskAssessmentSubHazard.DeleteRiskAssessmentSubHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, SubHazardid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvRASubHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRASubHazard.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
