using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class Registers_RegisterGST : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            MenuRegistersGST.AccessRights = this.ViewState;
            MenuRegistersGST.Title = "GST";
            MenuRegistersGST.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                txtaddresscode.Text = ViewState["ADDRESSCODE"].ToString();
                gvGST.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                SupplierGSTMappingEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersSupplierGST.GSTSearch(
                    int.Parse(txtaddresscode.Text),
                    sortexpression, sortdirection,
                    int.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvGST.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        gvGST.DataSource = ds;
        gvGST.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void RegistersGST_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvGST_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Place = ((RadComboBox)e.Item.FindControl("ddlstateunionterritory")).Text;
                string GST = ((RadTextBox)e.Item.FindControl("txtaddGST")).Text;
                PhoenixRegistersSupplierGST.GSTInsert(int.Parse(txtaddresscode.Text), Place, GST);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string Place = ((RadTextBox)e.Item.FindControl("txtPlace")).Text;
                string GST = ((RadTextBox)e.Item.FindControl("txtGST")).Text;
                Guid? dtkey = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblid")).Text);

                PhoenixRegistersSupplierGST.GSTUpdate(dtkey, Place, GST);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid? dtkey = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblid")).Text);

                PhoenixRegistersSupplierGST.GSTDelete(dtkey);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvGST_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdAdd");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton Delete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (Delete != null) Delete.Visible = SessionUtil.CanAccess(this.ViewState, Delete.CommandName);

            ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            if (e.Item is GridFooterItem)
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
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
    protected void gvGST_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void Rebind()
    {
        gvGST.SelectedIndexes.Clear();
        gvGST.EditIndexes.Clear();
        gvGST.DataSource = null;
        gvGST.Rebind();
    }

    private void SupplierGSTMappingEdit()
    {
        DataTable dt = PhoenixRegistersSupplierGST.SupplierGSTMappingEdit(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString()));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            txtSupplierName.Text = dr["FLDNAME"].ToString();
            txtSupplierCode.Text = dr["FLDCODE"].ToString();

        }
    }
    protected void gvGST_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvGST.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //private bool Validate(int addresscode
    //                        , string state
    //                        , int gstnumber)
    //{
    //    ucError.HeaderMessage = "Please provide the following required information";


    //    if (ucError.ErrorMessage != "")
    //        ucError.Visible = true;

    //    return (!ucError.IsError);
    //}

}