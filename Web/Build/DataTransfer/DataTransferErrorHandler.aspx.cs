using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.DataTransfer;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Telerik.Web.UI;

public partial class DataTransfer_DataTransferErrorHandler : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //binddata();
    }

    private void BindData()
    {
        DataSet ds = PhoenixDataTransferCommon.DataTransferImportCheck(int.Parse(Request.QueryString["vesselid"].ToString()));

        DataRow dr = ds.Tables[0].Rows[0];
        lbltitle.Text = dr["FLDTITLE"].ToString();
        lblsteps.Text = dr["FLDMESSAGE"].ToString();
        lblFileList.Text = dr["FLDDELETEFILE"].ToString();
        txtMissingSeq.Text = dr["FLDMISSING"].ToString();
        lblsteps.Attributes.Add("style", "overflow :hidden");
        lblvesseldetails.Text = "Data Synchronizer Log - [" + dr["FLDVESSELNAME"].ToString() + "] - " + " [" + dr["FLDVESSELID"].ToString() + "]";
        lbllastimport.Text = dr["FLDLASTIMPORT"].ToString();
        string missingseq = dr["FLDIMPORTMISSING"].ToString();

        gvMissingSeq.DataSource = ds.Tables[0];
        gvImportFolder.DataSource = ds.Tables[1];
        gvDeleteFileList.DataSource = ds.Tables[2];
    }

    protected void gvMissingSeq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvImportFolder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvDeleteFileList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

}
