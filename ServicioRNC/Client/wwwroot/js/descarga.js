
    function downloadFile(content, fileName) {
        const blob = new Blob([content], {type: 'application/zip' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
    }
