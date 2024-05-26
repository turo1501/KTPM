import time
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from webdriver_manager.chrome import ChromeDriverManager

options = Options()
options.add_experimental_option('detach', True)

driver = webdriver.Chrome(service=Service(ChromeDriverManager().install()),
                          options=options)

driver.get("http://localhost:50258/")
driver.maximize_window()

links = driver.find_elements("xpath","//a[@href]")  #tat ca elements
for link in links:
    if "Nike SB Zoom Blazer Mid EK" in link.get_attribute("innerHTML"):
        link.click()



links = driver.find_elements("xpath","//a[@href]")
for link in links:
    if "mua ngay" in link.get_attribute("innerHTML"):
        link.click()
        break



book_links = driver.find_elements("xpath",
                                  "//div[contains(@class,'elementor-column-wrap')][.//h2[text()[contains(.,'Nike SB Zoom Blazer Mid EK')]]][count(.//a)=2]//a")


book_links[0].click()