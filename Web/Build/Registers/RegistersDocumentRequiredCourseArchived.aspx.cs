using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Registers_RegistersDocumentRequiredCourseArchived : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvArchivedDocuments.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixRegistersVesselDocumentCourse.DocumentsRequiredSearch(General.GetNullableInteger(Filter.CurrentVesselMasterFilter),
                                                                                    General.GetNullableInteger(ucRank.SelectedRank), null,
                                                                                    null, null, 0, sortexpression, sortdirection,
                                                                                    Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvArchivedDocuments.PageSize,
                                                                                    ref iRowCount, ref iTotalPageCount);
        gvArchivedDocuments.DataSource = ds;
        gvArchivedDocuments.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void Rebind()
    {
        gvArchivedDocuments.SelectedIndexes.Clear();
        gvArchivedDocuments.EditIndexes.Clear();
        gvArchivedDocuments.DataSource = null;
        gvArchivedDocuments.Rebind();
    }
    private void UpdateDocumentsRequired(string vesseldocumentd, string effectivedate, string validtilldate)
    {
        PhoenixRegistersVesselDocumentCourse.UpdateArchiveDocumentsRequired(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Convert.ToInt32(vesseldocumentd), General.GetNullableDateTime(effectivedate), General.GetNullableDateTime(validtilldate));
    }
    protected void gvArchivedDocuments_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateDocumentsRequired(
                    ((RadLabel)e.Item.FindControl("lblVesselDocumentIdEdit")).Text
                    , ((UserControlDate)e.Item.FindControl("txtEffectiveDateEdit")).Text
                    , ((UserControlDate)e.Item.FindControl("txtValidTillDateEdit")).Text
                    );
                ucStatus.Text = "Document information updated";
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DEARCHIVE"))
            {

                Guid dtkey = new Guid(((RadLabel)e.Item.FindControl("lblDTKey")).Text);
                PhoenixRegistersVesselDocumentCourse.ArchiveDocumentsRequired(Int16.Parse(Filter.CurrentVesselMasterFilter), dtkey, 1, null, null);
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
    protected void gvArchivedDocuments_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdDeArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");

                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }
            LinkButton dbedit = (LinkButton)e.Item.FindControl("cmdEdit");

            if (dbedit != null)
            {
                dbedit.Visible = SessionUtil.CanAccess(this.ViewState, dbedit.CommandName);
            }
          
        }
    }
    protected void gvArchivedDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvArchivedDocuments.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
