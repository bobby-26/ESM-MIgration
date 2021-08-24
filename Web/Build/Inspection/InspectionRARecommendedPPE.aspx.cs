using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using SouthNests.Phoenix.Integration;

public partial class InspectionRARecommendedPPE : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRARecommendedPPE.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRAMiscellaneous')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRARecommendedPPE.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRAMiscellaneous.AccessRights = this.ViewState;
            MenuRAMiscellaneous.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                // ddlType.SelectedValue = Request.QueryString["type"];
                ddlType.Visible = false;
                ViewState["TYPE"] = "5";
        
                ucTitle.Text = "Recommended PPE";
                gvRAMiscellaneous.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDNAME" };
        string[] alCaptions = { "Name" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentMiscellaneousExtn.RiskAssessmentMiscellaneousSearch(General.GetNullableString(txtName.Text),
            General.GetNullableInteger(ViewState["TYPE"].ToString()),
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=" + ucTitle.Text + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> " + ucTitle.Text + "</h3></td>");
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

    protected void RAMiscellaneous_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRAMiscellaneous.Rebind();
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

    protected void Rebind()
    {
        gvRAMiscellaneous.SelectedIndexes.Clear();
        gvRAMiscellaneous.EditIndexes.Clear();
        gvRAMiscellaneous.DataSource = null;
        gvRAMiscellaneous.Rebind();
    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME" };
        string[] alCaptions = { "Name" };


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRiskAssessmentMiscellaneousExtn.RiskAssessmentMiscellaneousSearch(General.GetNullableString(txtName.Text),
            General.GetNullableInteger(ViewState["TYPE"].ToString()),
            sortexpression, sortdirection,
            gvRAMiscellaneous.CurrentPageIndex+1,
            gvRAMiscellaneous.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvRAMiscellaneous", "Recommended PPE", alCaptions, alColumns, ds);
        gvRAMiscellaneous.DataSource = ds;
        gvRAMiscellaneous.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }



    protected void gvRAMiscellaneous_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidRAMiscellaneous(
                        ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                        ((UserControlMaskNumber)e.Item.FindControl("ucScoreAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    InsertRAMiscellaneous(
                        ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                        ((UserControlMaskNumber)e.Item.FindControl("ucScoreAdd")).Text
                    );
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
                }

                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (!IsValidRAMiscellaneous(
                ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                ((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateRAMiscellaneous(
                            Int32.Parse(((Label)e.Item.FindControl("lblMiscellaneousIdEdit")).Text),
                             ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                             ((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text
                         );
                }

                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteDesignation(Int32.Parse(((Label)e.Item.FindControl("lblMiscellaneousId")).Text));
                }

                if (e.CommandName.ToUpper().Equals("IMAGEMAPPING"))
                {
                    string uniqueid = ((Label)e.Item.FindControl("lblMiscellaneousId")).Text;
                    Response.Redirect("../Inspection/InspectionRARecommendedPPEAdd.aspx?PPEID=" + uniqueid);
                }

            if (e.CommandName == "page")
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

    protected void gvRAMiscellaneous_ItemDataBound(object sender, GridItemEventArgs e)
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
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());

            Label lblMiscellaneousId = (Label)e.Item.FindControl("lblMiscellaneousId");
            HtmlTable tblForms = (HtmlTable)e.Item.FindControl("tblForms");
            if (lblMiscellaneousId != null)
            {
                DataSet dss = PhoenixInspectionRiskAssessmentMiscellaneousExtn.PPEEPSSList(lblMiscellaneousId.Text == null ? null : General.GetNullableInteger(lblMiscellaneousId.Text));
                int number = 1;
                if (dss.Tables[0].Rows.Count > 0)
                {
                    tblForms.Rows.Clear();
                    foreach (DataRow dr in dss.Tables[0].Rows)
                    {
                        HyperLink hl = new HyperLink();
                        hl.Text = dr["FLDNAME"].ToString();
                        hl.ID = "hlink" + number.ToString();
                        hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                        int type = 0;

                        PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDEPSSID"].ToString()), ref type);
                        if (type == 5)
                        {
                            DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(dr["FLDEPSSID"].ToString()));

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow drr = ds.Tables[0].Rows[0];

                                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDEPSSID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
                            }
                        }
                        else if (type == 6)
                        {
                            DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(dr["FLDEPSSID"].ToString()));

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow drr = ds.Tables[0].Rows[0];
                                if (drr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(drr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                                {
                                    DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drr["FLDFORMREVISIONDTKEY"].ToString()));
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow drRow = dt.Rows[0];
                                        hl.Target = "_blank";
                                        hl.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                                    }
                                }
                            }
                        }

                        HtmlTableRow tr = new HtmlTableRow();
                        HtmlTableCell tc = new HtmlTableCell();
                        tr.Cells.Add(tc);
                        tc = new HtmlTableCell();
                        tc.Controls.Add(hl);
                        tr.Cells.Add(tc);
                        tblForms.Rows.Add(tr);
                        tc = new HtmlTableCell();
                        tc.InnerHtml = "<br/>";
                        tr.Cells.Add(tc);
                        tblForms.Rows.Add(tr);
                        number = number + 1;
                    }
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    private void InsertRAMiscellaneous(string name, string score)
    {

        PhoenixInspectionRiskAssessmentMiscellaneousExtn.InsertRiskAssessmentMiscellaneous(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                  name,
                                                                  Convert.ToInt32(ViewState["TYPE"].ToString()),
                                                                  General.GetNullableInteger(score)
                                                                  );
    }

    private void UpdateRAMiscellaneous(int Miscellaneousid, string name, string score)
    {

        PhoenixInspectionRiskAssessmentMiscellaneousExtn.UpdateRiskAssessmentMiscellaneous(
             PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Miscellaneousid,
             name,
             Convert.ToInt32(ViewState["TYPE"].ToString()),
             General.GetNullableInteger(score)
             );
        ucStatus.Text = "Information updated";
    }

    private bool IsValidRAMiscellaneous(string name, string score)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        // GridView _gridView = gvRAMiscellaneous;

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";


        return (!ucError.IsError);
    }

    private void DeleteDesignation(int Miscellaneousid)
    {
        PhoenixInspectionRiskAssessmentMiscellaneousExtn.DeleteRiskAssessmentMiscellaneous(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Miscellaneousid);
    }

    protected void gvRAMiscellaneous_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRAMiscellaneous.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAMiscellaneous_SortCommand(object sender, GridSortCommandEventArgs e)
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
}