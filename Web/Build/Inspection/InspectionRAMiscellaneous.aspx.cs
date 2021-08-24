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

public partial class InspectionRAMiscellaneous : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAMiscellaneous.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRAMiscellaneous')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAMiscellaneous.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRAMiscellaneous.AccessRights = this.ViewState;
            MenuRAMiscellaneous.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TYPE"] = Request.QueryString["type"];
                // ddlType.SelectedValue = Request.QueryString["type"];
                ddlType.Visible = false;
                ViewState["type"] = "";
                if (Request.QueryString["type"] == "1")
                {
                    ucTitle.Text = "Reason";
                }
                else if (Request.QueryString["type"] == "3")
                {
                    ucTitle.Text = "Impact Type";
                    ViewState["type"] = Request.QueryString["type"];
                }
                else if (Request.QueryString["type"] == "4")
                {
                    ucTitle.Text = "Potential Consequences";
                }
                else if (Request.QueryString["type"] == "5")
                {
                    ucTitle.Text = "Recommended PPE";
                }
                else if (Request.QueryString["type"] == "6")
                {
                    ucTitle.Text = "Activity";
                }
            }
            BindData();
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

        if (General.GetNullableInteger(ViewState["type"].ToString()) != null && General.GetNullableInteger(ViewState["type"].ToString()) == 3)
        {
            alColumns = new string[] { "FLDNAME", "FLDSCORE" };
            alCaptions = new string[] { "Name", "Score" };
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentMiscellaneous.RiskAssessmentMiscellaneousSearch(General.GetNullableString(txtName.Text),
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
                ViewState["PAGENUMBER"] = 1;
                BindData();
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

        string[] alColumns = { "FLDNAME" };
        string[] alCaptions = { "Name" };

        if (General.GetNullableInteger(ViewState["type"].ToString()) != null && General.GetNullableInteger(ViewState["type"].ToString()) == 3)
        {
            alColumns = new string[] { "FLDNAME", "FLDSCORE" };
            alCaptions = new string[] { "Name", "Score" };
            gvRAMiscellaneous.Columns[1].Visible = true;
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRiskAssessmentMiscellaneous.RiskAssessmentMiscellaneousSearch(General.GetNullableString(txtName.Text),
            General.GetNullableInteger(ViewState["TYPE"].ToString()),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvRAMiscellaneous", "Reason", alCaptions, alColumns, ds);

        gvRAMiscellaneous.DataSource = ds;
        gvRAMiscellaneous.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        LinkButton ib = (LinkButton)sender;
        BindData();
    }

    protected void gvRAMiscellaneous_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
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
                    gvRAMiscellaneous.Rebind();
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
                }
                BindData();
                gvRAMiscellaneous.Rebind();
            }
             if (e.Item is GridEditableItem)
            {
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

                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteDesignation(Int32.Parse(((Label)e.Item.FindControl("lblMiscellaneousId")).Text));
                }
                BindData();
                gvRAMiscellaneous.Rebind();
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

        PhoenixInspectionRiskAssessmentMiscellaneous.InsertRiskAssessmentMiscellaneous(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                  name,
                                                                  Convert.ToInt32(ViewState["TYPE"].ToString()),
                                                                  General.GetNullableInteger(score)
                                                                  );
    }

    private void UpdateRAMiscellaneous(int Miscellaneousid, string name, string score)
    {

        PhoenixInspectionRiskAssessmentMiscellaneous.UpdateRiskAssessmentMiscellaneous(
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

        if (General.GetNullableInteger(ViewState["type"].ToString()) != null && General.GetNullableInteger(ViewState["type"].ToString()) == 3)
        {
            if (General.GetNullableInteger(score) == null)
                ucError.ErrorMessage = "Score is required.";
        }

        return (!ucError.IsError);
    }

    private void DeleteDesignation(int Miscellaneousid)
    {
        PhoenixInspectionRiskAssessmentMiscellaneous.DeleteRiskAssessmentMiscellaneous(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Miscellaneousid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvRAMiscellaneous_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
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
