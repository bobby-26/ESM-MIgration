using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersVesselDirection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselDirection.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselDirection')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersVesselDirection.AccessRights = this.ViewState;
            MenuRegistersVesselDirection.MenuList = toolbar.Show();

            if (!IsPostBack)
            {                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvVesselDirection.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersVesselDirection_TabStripCommand(object sender, EventArgs e)
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
    protected void Rebind()
    {
        gvVesselDirection.SelectedIndexes.Clear();
        gvVesselDirection.EditIndexes.Clear();
        gvVesselDirection.DataSource = null;
        gvVesselDirection.Rebind();
    }
    private bool checkvalue(string name, string shortname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((shortname == null) || (shortname == ""))
            ucError.ErrorMessage = "Direction Short Name is required.";

        if ((name == null) || (name == ""))
            ucError.ErrorMessage = "Direction Name is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    protected void gvVesselDirection_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string DirectionShortName, DirectionName;

                DirectionName = (((RadTextBox)e.Item.FindControl("txtDirectionNameAdd")).Text);
                DirectionShortName = (((RadTextBox)e.Item.FindControl("txtDirectionShortNameAdd")).Text);
                if ((!checkvalue(DirectionName, DirectionShortName)))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersDirection.InsertDirection((PhoenixSecurityContext.CurrentSecurityContext.UserCode),
                                                            (((RadTextBox)e.Item.FindControl("txtDirectionNameAdd")).Text),
                                                             (((RadTextBox)e.Item.FindControl("txtDirectionShortNameAdd")).Text)
                                                          );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersDirection.DeleteDirection((PhoenixSecurityContext.CurrentSecurityContext.UserCode), General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDirectionCode")).Text));
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

    protected void gvVesselDirection_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            if (e.Item is GridEditableItem)
            {
                RadLabel lb = (RadLabel)e.Item.FindControl("lblShowYN");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (lb != null)
                    lb.Text = drv["FLDSHOWYESNO"].ToString().Equals("1") ? "Yes" : "No";

                //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                //{
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

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
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSHORTNAME", "FLDDIRECTIONNAME" };
        string[] alCaptions = { "Direction Short Name", "Direction Name" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixRegistersDirection.DirectionSearch("","",
                                                 sortexpression,
                                                 sortdirection,
                                                 int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                 gvVesselDirection.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount
                                               );

        General.SetPrintOptions("gvVesselDirection", "Direction Registers", alCaptions, alColumns, ds);

        gvVesselDirection.DataSource = ds;
        gvVesselDirection.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alCaptions = new string[2];
        string[] alColumns = { "FLDSHORTNAME", "FLDDIRECTIONNAME" };

        alCaptions[0] = "Direction Short Name";
        alCaptions[1] = "Direction Name";

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersDirection.DirectionSearch("", "",
                                                  sortexpression,
                                                  sortdirection,
                                                  1,
                                                  iRowCount,
                                                  ref iRowCount,
                                                  ref iTotalPageCount
                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"Direction.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Direction</h3></td>");
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

    protected void gvVesselDirection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselDirection.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselDirection_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselDirection_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!checkvalue((((RadTextBox)e.Item.FindControl("txtDirectionNameEdit")).Text), (((RadTextBox)e.Item.FindControl("txtDirectionShortNameEdit")).Text)))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersDirection.UpdateDirection(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDirectionCodeEdit")).Text),
                                                General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDirectionNameEdit")).Text),
                                                General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDirectionShortNameEdit")).Text)
                                              );
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
