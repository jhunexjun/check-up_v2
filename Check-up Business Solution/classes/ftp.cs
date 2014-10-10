using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

class ftp
{
    private FtpWebRequest ftpRequest = null;
    private FtpWebResponse ftpResponse = null;
    private Stream ftpStream = null;
    private int bufferSize = 2048;

    private string _username, _password, _urlAndFileName, _filePath;
    
    internal ftp(string username, string password)
    {
        _username = username;
        _password = password;
    }

    internal string filePath
    {
        // where to get the file from
        set { _filePath = value; }
    }

    internal string urlAndFileName
    {
        //sample ftp://192.168.1.100/incoming/file.json
        set { _urlAndFileName = value; }
    }

    internal bool Upload()
    {
        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_urlAndFileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(_username, _password);

            StreamReader sourceStream = new StreamReader(_filePath);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
            return true;
        }
        catch
        {
            MessageBox.Show("Sending file " + _filePath + " was unsuccessful!");
            return false;
        }
    }

    // courtesy of http://www.codeproject.com/Tips/443588/Simple-Csharp-FTP-Class
    internal bool download(string remoteFile, string localFile)
    {
        _urlAndFileName = remoteFile;

        try
        {
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(_urlAndFileName);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(_username, _password);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Get the FTP Server's Response Stream */
            ftpStream = ftpResponse.GetResponseStream();
            /* Open a File Stream to Write the Downloaded File */
            FileStream localFileStream = new FileStream(localFile, FileMode.Create);
            /* Buffer for the Downloaded Data */
            byte[] byteBuffer = new byte[bufferSize];
            int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
            /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
            try
            {
                while (bytesRead > 0)
                {
                    localFileStream.Write(byteBuffer, 0, bytesRead);
                    bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); return false; }
            
            /* Resource Cleanup */
            localFileStream.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
            return false;
        }
        return true;
    }
}
