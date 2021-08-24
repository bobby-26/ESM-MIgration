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
public partial class RegistersCargoType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCargoType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCargoType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersCargoType.AccessRights = this.ViewState;
            MenuRegistersCargoType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] =null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCargoType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvCargoType.EditIndexes.Clear();
        gvCargoType.SelectedIndexes.Clear();
        gvCargoType.DataSource = null;
        gvCargoType.Rebind();
    }
    protected void RegistersCargoType_TabStripCommand(object sender, EventArgs e)
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

    protected void gvCargoType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("ucVesselTypeListAdd");
                string vesseltypelist = "";
                foreach (ButtonListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        vesseltypelist += li.Value + ",";
                    }
                }
                if (General.GetNullableString(vesseltypelist) != null)
                    vesseltypelist = "," + vesseltypelist;

                if (!checkvalue((((RadTextBox)e.Item.FindControl("txtCargoTypeNameAdd")).Text)))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersCargo.InsertCargoType((PhoenixSecurityContext.CurrentSecurityContext.UserCode),
                                                            (((RadTextBox)e.Item.FindControl("txtCargoTypeNameAdd")).Text),
                                                            General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkShowYN")).Checked.Equals(true) == false ? "0" : "1"),
                                                            General.GetNullableString(vesseltypelist)
                                                          );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersCargo.DeleteCargoType((PhoenixSecurityContext.CurrentSecurityContext.UserCode),General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCargoTypeCode")).Text));
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

    private bool checkvalue(string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((type == null) || (type == "") || (type == "Dummy"))
            ucError.ErrorMessage = "Cargo Type is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCARGOTYPENAME", "FLDTYPEDESCRIPTION", "FLDSHOWYESNOINWORD" };
        string[] alCaptions = { "Cargo Type", "Vessel Type", "Show Y/N" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixRegistersCargo.CargoTypeSearch(General.GetNullableInteger(""),"",sortexpression, sortdirection,int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCargoType.PageSize, ref iRowCount,ref iTotalPageCount
                                                   );

        General.SetPrintOptions("gvCargoType", "Cargo Type", alCaptions, alColumns, ds);

        gvCargoType.DataSource = ds;
        gvCargoType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvCargoType_ItemDataBound(Object sender, GridItemEventArgs e)
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

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
              
                //if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                //{
                    RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkShowYN");                    
                    if (cb != null)
                        cb.Checked = drv["FLDSHOWYESNO"].ToString().Equals("1") ? true : false;
                //}

                RadCheckBoxList ucVesselTypeList = (RadCheckBoxList)e.Item.FindControl("ucVesselTypeListEdit");
                if (ucVesselTypeList != null)
                {
                    ucVesselTypeList.DataSource = PhoenixRegistersHard.ListHard(1, 81);
                    ucVesselTypeList.DataBindings.DataTextField = "FLDHARDNAME";
                    ucVesselTypeList.DataBindings.DataValueField = "FLDHARDCODE";
                    ucVesselTypeList.DataBind();

                    RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("ucVesselTypeListEdit");
                    foreach (ButtonListItem li in chk.Items)
                    {
                        string[] slist = drv["FLDVESSELTYPELIST"].ToString().Split(',');
                        foreach (string s in slist)
                        {
                            if (li.Value.Equals(s))
                            {
                                li.Selected = true;
                            }
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

                RadCheckBoxList ucVesselTypeList = (RadCheckBoxList)e.Item.FindControl("ucVesselTypeListAdd");
                if (ucVesselTypeList != null)
                {
                    ucVesselTypeList.DataSource = PhoenixRegistersHard.ListHard(1, 81);
                    ucVesselTypeList.DataBindings.DataTextField = "FLDHARDNAME";
                    ucVesselTypeList.DataBindings.DataValueField = "FLDHARDCODE";
                    ucVesselTypeList.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCARGOTYPENAME", "FLDTYPEDESCRIPTION", "FLDSHOWYESNOINWORD" };
        string[] alCaptions = { "Cargo Type", "Vessel Type", "Show Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCargo.CargoTypeSearch(General.GetNullableInteger(""), "", sortexpression, sortdirection, 1,
                                                    iRowCount, ref iRowCount, ref iTotalPageCount
                                                   );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"CargoType.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Cargo Type</h3></td>");
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
    protected void gvCargoType_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvCargoType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCargoType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCargoType_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string yn = ((RadCheckBox)e.Item.FindControl("chkShowYN")).Checked == true ? "1" : "0";

            if (!checkvalue((((RadTextBox)e.Item.FindControl("txtCargoTypeNameEdit")).Text)))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("ucVesselTypeListEdit");
            string vesseltypelist = "";
            foreach (ButtonListItem li in chk.Items)
            {
                if (li.Selected)
                {
                    vesseltypelist += li.Value + ",";
                }
            }
            if (General.GetNullableString(vesseltypelist) != null)
                vesseltypelist = "," + vesseltypelist;

            PhoenixRegistersCargo.UpdateCargoType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                               General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCargoTypeCodeEdit")).Text),
                                               General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCargoTypeNameEdit")).Text),
                                               General.GetNullableInteger(yn),
                                               General.GetNullableString(vesseltypelist));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
