using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Document;
using Telerik.Web.UI;

public partial class PhoenixDataDictionary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);

                DataSet TblSearch = new DataSet();
                TblSearch = SouthNests.Phoenix.Document.PhoenixDataDictionary.TableSearch();
                TableDropDown.DataSource = TblSearch.Tables[0];
                TableDropDown.DataTextField = "NAME";
                TableDropDown.DataValueField = "NAME";
                TableDropDown.DataBind();

                ColumnSearchButton.Visible = false;

                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBER1"] = 1;
                gvTableColumnSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   protected void BindData()
    {
        int iRowCount = 15;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        ds = SouthNests.Phoenix.Document.PhoenixDataDictionary.ColumnSearch(TableDropDown.SelectedValue, (int)ViewState["PAGENUMBER"],
            gvTableColumnSearch.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        gvTableColumnSearch.DataSource = ds;
        gvTableColumnSearch.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
   }
    private bool IsValidData(string ColumnDescription)
    {
        if (ColumnDescription.ToString().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }
    protected void gvTableColumnSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTableColumnSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvTableColumnSearch.SelectedIndexes.Clear();
        gvTableColumnSearch.EditIndexes.Clear();
        gvTableColumnSearch.DataSource = null;
        gvTableColumnSearch.Rebind();
    }

    protected void gvTableColumnSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvTableColumnSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
        }
    }

    protected void gvTableColumnSearch_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (IsValidData(((RadTextBox)e.Item.FindControl("ColumnDescriptionTxtBoxEdit")).Text))
            {
                SouthNests.Phoenix.Document.PhoenixDataDictionary.ColumnDescriptionUpdate(
                                ((RadLabel)e.Item.FindControl("FlblTableName")).Text,
                                ((LinkButton)e.Item.FindControl("lnkColumnName")).Text,
                                ((RadTextBox)e.Item.FindControl("ColumnDescriptionTxtBoxEdit")).Text
                                 );
                SouthNests.Phoenix.Document.PhoenixDataDictionary.ColumnDescriptionUserNameUpdate(
                           PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           ((RadLabel)e.Item.FindControl("FlblTableName")).Text,
                           ((LinkButton)e.Item.FindControl("lnkColumnName")).Text,
                            0);
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ColumnSearchButton_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void TableDropDown_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        ViewState["PAGENUMBER1"] = 1;
        Rebind();
    }
}

