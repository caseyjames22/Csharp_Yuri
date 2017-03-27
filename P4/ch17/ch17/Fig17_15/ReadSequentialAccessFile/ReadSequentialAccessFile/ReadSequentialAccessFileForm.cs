﻿// Fig. 17.15: ReadSequentialAccessFileForm.cs
// Reading a sequential-access file using deserialization.
using System;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using BankLibrary;

namespace ReadSequentialAccessFile
{
   public partial class ReadSequentialAccessFileForm : BankUIForm
   {
      // object for deserializing RecordSerializable in binary format
      private BinaryFormatter reader = new BinaryFormatter();
      private FileStream input; // stream for reading from a file

      // parameterless constructor
      public ReadSequentialAccessFileForm()
      {
         InitializeComponent();
      } // end constructor

      // invoked when user clicks the Open button
      private void openButton_Click( object sender, EventArgs e )
      {
         // create and show dialog box enabling user to open file
         DialogResult result; // result of OpenFileDialog
         string fileName; // name of file containing data

         using ( OpenFileDialog fileChooser = new OpenFileDialog() )
         {
            result = fileChooser.ShowDialog();
            fileName = fileChooser.FileName; // get specified name
         } // end using

         // ensure that user clicked "OK"
         if ( result == DialogResult.OK )
         {
            ClearTextBoxes();

            // show error if user specified invalid file
            if ( fileName == string.Empty )
               MessageBox.Show( "Invalid File Name", "Error",
                  MessageBoxButtons.OK, MessageBoxIcon.Error );
            else
            {
               // create FileStream to obtain read access to file
               input = new FileStream(
                  fileName, FileMode.Open, FileAccess.Read );

               openButton.Enabled = false; // disable Open File button
               nextButton.Enabled = true;  // enable Next Record button
            } // end else
         } // end if
      } // end method openButton_Click

      // invoked when user clicks Next button
      private void nextButton_Click( object sender, EventArgs e )
      {
         // deserialize RecordSerializable and store data in TextBoxes
         try
         {
            // get next RecordSerializable available in file
            RecordSerializable record =
               ( RecordSerializable ) reader.Deserialize( input );

            // store RecordSerializable values in temporary string array
            string[] values = new string[] {
               record.Account.ToString(),
               record.FirstName.ToString(),
               record.LastName.ToString(),
               record.Balance.ToString()
            };

            // copy string array values to TextBox values
            SetTextBoxValues( values );
         } // end try
         // handle exception when there are no RecordSerializables in file
         catch ( SerializationException )
         {
            input.Close(); // close FileStream 
            openButton.Enabled = true; // enable Open File button
            nextButton.Enabled = false; // disable Next Record button

            ClearTextBoxes();

            // notify user if no RecordSerializables in file
            MessageBox.Show( "No more records in file", string.Empty,
               MessageBoxButtons.OK, MessageBoxIcon.Information );
         } // end catch
      } // end method nextButton_Click
   } // end class ReadSequentialAccessFileForm
} // end namespace ReadSequentialAccessFile

