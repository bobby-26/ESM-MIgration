using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersVesselCertificatesArchieve : PhoenixBasePage
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
                gvVesselCertificatesArchieve.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME" };
        string[] alCaptions = { "Certificate", "Number", "Issue Date", "Expiry Date", "Authority" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersVesselCertificates.VesselCertificatesSearch(Int16.Parse(Filter.CurrentVesselMasterFilter), null, "", null, null, null, "", 0, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvVesselCertificatesArchieve.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvVesselCertificates", "Certificates", alCaptions, alColumns, ds);

        gvVesselCertificatesArchieve.DataSource = ds;
        gvVesselCertificatesArchieve.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
  
    protected void Rebind()
    {
        gvVesselCertificatesArchieve.SelectedIndexes.Clear();
        gvVesselCertificatesArchieve.EditIndexes.Clear();
        gvVesselCertificatesArchieve.DataSource = null;
        gvVesselCertificatesArchieve.Rebind();
    }
    protected void gvVesselCertificatesArchieve_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ARCHIVE"))
            {                
                string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                PhoenixRegistersVesselCertificates.ArchiveVesselCertificates(Int16.Parse(Filter.CurrentVesselMasterFilter), dtkey, 1);

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
    protected void gvVesselCertificatesArchieve_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselCertificatesArchieve.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVesselCertificatesArchieve_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
            if (att != null)
            {
                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod=" + PhoenixModule.REGISTERS + "'); return true;");
            }
            RadLabel expdate = e.Item.FindControl("lblDateOfExpiry") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    //e.Row.CssClass = "rowred";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }

           
        }
    }
}
