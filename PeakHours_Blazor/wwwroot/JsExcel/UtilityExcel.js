function DownloadExcelFile(fileName, data) {
    const link = document.createElement('a');
    link.download = fileName;
    link.href = "data:application/vnd.ms-excel;base64," + data;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
