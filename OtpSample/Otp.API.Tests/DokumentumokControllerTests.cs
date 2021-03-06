﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Otp.API.Controllers;
using Otp.API.Models;
using Otp.API.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Otp.API.Tests
{
    public class DokumentumokControllerTests
    {
        private const string DokumentumokPath = @"c:\Dokumentumok\";
        private readonly Mock<IOptions<DokumentumokConfiguration>> _options;
        private readonly Mock<ILogger<DokumentumokController>> _logger;

        public DokumentumokControllerTests()
        {
            DokumentumokConfiguration dokumentumokConfiguration = new DokumentumokConfiguration() { Path = DokumentumokPath };
            _options = new Mock<IOptions<DokumentumokConfiguration>>();
            _options.Setup(conf => conf.Value).Returns(dokumentumokConfiguration);

            _logger = new Mock<ILogger<DokumentumokController>>();
        }

        [Fact]
        public void DokumentumokController_ILoggerIsNull_ThrowsNullReferenceException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);

            // Assert
            Assert.Throws<NullReferenceException>(() => new DokumentumokController(null, dokumentumokService));
        }

        [Fact]
        public void DokumentumokController_IDokumentumokServiceIsNull_ThrowsNullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() => new DokumentumokController(_logger.Object, null));
        }

        [Fact]
        public void GetDokumentumok_DirectoryIsEmpty_ReturnsOk()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            
            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService);

            // Act
            var result = controller.GetDokumentumok();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetDokumentumok_DirectoryIsNotEmpty_ReturnsAllItems()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory((Path.Combine(DokumentumokPath, "empty-sub-dir")));
            fileSystem.AddFile(Path.Combine(DokumentumokPath, @"sub-dir\test2.txt"), new MockFileData("Lorem ipsum."));
            fileSystem.AddFile(Path.Combine(DokumentumokPath, "test.txt"), new MockFileData("Lorem ipsum."));

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService);

            // Act
            var result = controller.GetDokumentumok();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Equal(4, items.ToList().Count);
        }

        [Fact]
        public void GetDokumentumok_DirectoryIsEmpty_ReturnsEmptyIEnumerable()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService);

            // Act
            var result = controller.GetDokumentumok();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Empty(items);
        }

        [Fact]
        public async Task GetDokumentum_InvalidFileRequest_ReturnsNotFound()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileName = "non-existing.txt";
            fileSystem.AddDirectory(DokumentumokPath);

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            controller.ControllerContext.HttpContext.Request.Method = "GET";
            var cancellationToken = new CancellationToken();

            // Act
            var result = await controller.GetDokumentum(fileName, cancellationToken);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetDokumentum_InvalidFileSizeRequest_ReturnsNotFound()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileName = "non-existing.txt";
            fileSystem.AddDirectory(DokumentumokPath);

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            controller.ControllerContext.HttpContext.Request.Method = "HEAD";

            // Act
            var result = controller.GetDokumentum(fileName);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetDokumentum_ValidFileRequest_ReturnsConvertedFile()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileName = "test.txt";
            var fileData = "Lorem ipsum.";
            fileSystem.AddFile(Path.Combine(DokumentumokPath, fileName), new MockFileData(fileData));

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            controller.ControllerContext.HttpContext.Request.Method = "GET";

            string expectedData = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileData));
            var cancellationToken = new CancellationToken();

            // Act
            var result = await controller.GetDokumentum(fileName, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultData = Assert.IsType<string>(okResult.Value);
            Assert.Equal(expectedData, resultData);
        }

        [Fact]
        public void GetDokumentum_ValidFileSizeRequest_ReturnsFileSizeAsContentLength()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileName = "test.txt";
            var fileData = "Lorem ipsum.";
            var filePath = (Path.Combine(DokumentumokPath, fileName));
            fileSystem.AddFile(filePath, new MockFileData(fileData));

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            controller.ControllerContext.HttpContext.Request.Method = "HEAD";

            var expectedFileSize = fileSystem.FileInfo.FromFileName(filePath).Length;

            // Act
            var result = controller.GetDokumentum(fileName);
            var actualFileSize = controller.Response.Headers["Content-Length"];

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(expectedFileSize, Convert.ToInt64(actualFileSize));
        }

        /// <summary>
        /// The System.IO.Abstractions.TestingHelpers doesn't abort when the Token is cancelled.
        /// But the System.IO.Abstractions works as expected.
        /// Bug has been reported. Assertation have to be modified to empty string check, when the bug will be fixed.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetDokumentum_CancellationRequested_ReturnsEmptyString()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var fileName = "test.txt";
            var fileData = "Lorem ipsum.";
            fileSystem.AddFile(Path.Combine(DokumentumokPath, fileName), new MockFileData(fileData));

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            controller.ControllerContext.HttpContext.Request.Method = "GET";
            
            var cancellationToken = new CancellationToken(true);

            // Act
            var result = await controller.GetDokumentum(fileName, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultData = Assert.IsType<string>(okResult.Value);
            Assert.NotNull(resultData);
        }

        [Theory]
        [InlineData("", "upload-empty-file.txt")]
        [InlineData("Sub-folder", @"Sub-folder\upload-empty-file.txt")]
        public async Task PostDokumentum_PassValidFile_ReturnsCreatedAtRouteResult(string createDirectory, string fileName)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(Path.Combine(DokumentumokPath, createDirectory));

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService);
            var cancellationToken = new CancellationToken();

            // Act
            var result = await controller.PostDokumentum(fileName, "", cancellationToken);
            
            // Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        [Theory]
        [InlineData("upload-in:valid-file.txt")]
        [InlineData(@"Non-existing-sub-folder\upload-empty-file.txt")]
        public async Task PostDokumentum_PassInvalidFile_ReturnsBadRequest(string fileName)
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService);
            var cancellationToken = new CancellationToken();

            // Act
            var result = await controller.PostDokumentum(fileName, "", cancellationToken);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // The System.IO.Abstractions.TestingHelpers doesn't abort when the Token is cancelled.
        // Bug has been reported, when it will be fixed this test can be uncommented.
        /*
        [Fact]
        public async Task PostDokumentum_CancellationRequested_ReturnsEmptyString()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            var fileName = "upload-empty-file.txt";

            var dokumentumokService = new DokumentumokService(_options.Object, fileSystem);
            var controller = new DokumentumokController(_logger.Object, dokumentumokService);
            var cancellationToken = new CancellationToken(true);
            
            // Act
            var result = await controller.PostDokumentum(fileName, "", cancellationToken);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        */
    }
}
